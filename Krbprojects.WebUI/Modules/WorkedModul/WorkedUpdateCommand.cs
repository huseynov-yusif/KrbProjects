using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Krbprojects.WebUI.AppCode.Extensions;
using Krbprojects.WebUI.Areas.KrbAdmin.Models.ViewModels;
using Krbprojects.WebUI.Models.DbContexts;
using Krbprojects.WebUI.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Krbprojects.WebUI.Modules.WorkedModul
{
    public class WorkedUpdateCommand : WorkedViewModel, IRequest<long>
    {
        public class WorkedUpdateCommandHandler : IRequestHandler<WorkedUpdateCommand, long>
        {
            readonly KrbProjectsBaseDbContext db;
            readonly IActionContextAccessor ctx;
            readonly IWebHostEnvironment env;
            public WorkedUpdateCommandHandler(KrbProjectsBaseDbContext db, IActionContextAccessor ctx, IWebHostEnvironment env)
            {
                this.ctx = ctx;
                this.db = db;
                this.env = env;
            }

            public async Task<long> Handle(WorkedUpdateCommand request, CancellationToken cancellationToken)
            {

                if (request.Id == null || request.Id < 0)
                    return 0;

                if (ctx.IsModelStateValid())
                {
                    var entity = await db.Workeds
                       .Include(p => p.Images.Where(pi => pi.DeletedByUserId == null))
                       .FirstOrDefaultAsync(p => p.Id == request.Id && p.DeletedByUserId == null);
                    entity.Name_AZ = request.Name_AZ;
                    entity.Name_EN = request.Name_EN;
                    entity.Name_RU = request.Name_RU;
                    entity.Description_AZ = request.Description_AZ;
                    entity.Description_EN = request.Description_EN;
                    entity.Description_RU = request.Description_RU;

                    if (entity == null)
                    {
                        return 0;
                    }

                    /// Silinmishler ---start
                    int[] deletedImageIds = request.Files.Where(p => p.Id > 0 && string.IsNullOrWhiteSpace(p.TempPath))
                                                    .Select(p => p.Id.Value)
                                                    .ToArray();

                    foreach (var itemId in deletedImageIds)
                    {
                        var oldImage = await db.WorkedImages.FirstOrDefaultAsync(pi => pi.WorkedId == entity.Id && pi.Id == itemId);

                        if (oldImage == null)
                            continue;

                        oldImage.DeletedDate = DateTime.Now;
                        oldImage.DeletedByUserId = 1;
                    }
                    /// Silinmishler ---end

                    foreach (var item in request.Files.Where(f => (f.Id > 0 && !string.IsNullOrWhiteSpace(f.TempPath)) //bazada olub deyismeyenler
                    || (f.File != null && f.Id == null)))///yeni yarananlar
                    {
                        if (item.File == null) //deyismeyenler
                        {
                            var oldImage = await db.WorkedImages.FirstOrDefaultAsync(pi => pi.WorkedId == entity.Id && pi.Id == item.Id);

                            if (oldImage == null)
                                continue;

                            oldImage.IsMain = item.IsMain;
                        }
                        else if (item.File != null) //yeni elave olunanlar
                        {
                            string extension = Path.GetExtension(item.File.FileName);
                            string fileName = $"{Guid.NewGuid()}{extension}";

                            string physicalFileName = Path.Combine(env.ContentRootPath,
                                                                  "wwwroot",
                                                                  "uploads",
                                                                  "ProjectPhotos",
                                                                   fileName);

                            using (var stream = new FileStream(physicalFileName, FileMode.Create, FileAccess.Write))
                            {
                                await item.File.CopyToAsync(stream);
                            }

                            entity.Images.Add(new WorkedImage
                            {
                                FileName = fileName,
                                IsMain = item.IsMain
                            });

                        }
                    }

                    await db.SaveChangesAsync();

                    return entity.Id;
                }
                return 0;

            }
        }
    }
}
