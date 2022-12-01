using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Krbprojects.WebUI.AppCode.Extensions;
using Krbprojects.WebUI.Areas.KrbAdmin.Models;
using Krbprojects.WebUI.Models.DbContexts;
using Krbprojects.WebUI.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Krbprojects.WebUI.Modules.WorkedModul
{
    public class WorkedCreateCommand : IRequest<long>
    {
        [Required]
        public string Name_AZ { get; set; }
        public string Name_EN { get; set; }
        public string Name_RU { get; set; }
        public string Description_AZ { get; set; }
        public string Description_EN { get; set; }
        public string Description_RU { get; set; }
       
        public ICollection<WorkedImage> Images { get; set; }
        public ImageItemFormModel[] images { get; set; }

        public class WorkedCreateCommandHandler : IRequestHandler<WorkedCreateCommand, long>
        {
            readonly KrbProjectsBaseDbContext db;
            readonly IActionContextAccessor ctx;
            readonly IWebHostEnvironment env;
            readonly IMapper mapper;
            public WorkedCreateCommandHandler(KrbProjectsBaseDbContext db, IActionContextAccessor ctx, IWebHostEnvironment env, IMapper mapper)
            {
                this.env = env;
                this.ctx = ctx;
                this.db = db;
                this.mapper = mapper;
            }
            public async Task<long> Handle(WorkedCreateCommand request, CancellationToken cancellationToken)
            {
                if (request.images == null || !request.images.Any(i => i.File != null))
                {
                    ctx.ActionContext.ModelState.AddModelError("Images", "There are not images");
                }

                if (ctx.IsModelStateValid())
                {

                    Worked work = new Worked();
                    work.Name_AZ = request.Name_AZ;
                    work.Name_EN = request.Name_EN;
                    work.Name_RU = request.Name_RU;
                    work.Images = request.Images;
                    work.Description_AZ = request.Description_AZ;
                    work.Description_EN = request.Description_EN;
                    work.Description_RU = request.Description_RU;

                    work.Images = new List<WorkedImage>();
                    foreach (var image in request.images.Where(i => i.File != null))
                    {
                        string extension = Path.GetExtension(image.File.FileName); //.jpg
                        string imagePath = $"{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid()}{extension}";
                        string physicalPath = Path.Combine(env.ContentRootPath,
                            "wwwroot",
                            "uploads",
                            "ProjectPhotos",
                            imagePath);

                        using (var stream = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                        {
                            await image.File.CopyToAsync(stream);
                        }

                        work.Images.Add(new WorkedImage
                        {
                            IsMain = image.IsMain,
                            FileName = imagePath
                        });
                    }

                    db.Workeds.Add(work);
                    await db.SaveChangesAsync(cancellationToken);

                   

                    return work.Id;
                }
                return 0;


            }
        }
    }
}
