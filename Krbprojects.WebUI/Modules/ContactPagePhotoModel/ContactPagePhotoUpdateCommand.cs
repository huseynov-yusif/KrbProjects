using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Krbprojects.WebUI.Areas.KrbAdmin.Models.ViewModels;
using Krbprojects.WebUI.Models.DbContexts;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Krbprojects.WebUI.Modules.ContactPagePhotoModel
{
    public class ContactPagePhotoUpdateCommand : ContactPagePhotoViewModel, IRequest<int>
    {
        public class ContactPagePhotoUpdateCommandHandler : IRequestHandler<ContactPagePhotoUpdateCommand, int>
        {
            readonly KrbProjectsBaseDbContext db;
            readonly IActionContextAccessor ctx;
            readonly IWebHostEnvironment env;
            public ContactPagePhotoUpdateCommandHandler(KrbProjectsBaseDbContext db, IActionContextAccessor ctx, IWebHostEnvironment env)
            {
                this.ctx = ctx;
                this.db = db;
                this.env = env;
            }
            public async Task<int> Handle(ContactPagePhotoUpdateCommand request, CancellationToken cancellationToken)
            {
                if (request.Id == null || request.Id <= 0)
                    return 0;

                if (string.IsNullOrWhiteSpace(request.fileTemp) && request.file == null)
                {
                    ctx.ActionContext.ModelState.AddModelError("file", "Image was added!");
                }


                var entity = await db.ContactPagePhotos.FirstOrDefaultAsync(b => b.Id == request.Id);

                if (entity == null)
                {
                    return 0;
                }

                entity.ImagePath = request.ImagePath;
                entity.Name = request.Name;

                if (request.file != null)
                {
                    string extension = Path.GetExtension(request.file.FileName);
                    request.ImagePath = $"{Guid.NewGuid()}{extension}";

                    string physicalFileName = Path.Combine(env.ContentRootPath,
                                                           "wwwroot",
                                                           "uploads",
                                                           "files",
                                                           request.ImagePath);

                    using (var stream = new FileStream(physicalFileName, FileMode.Create, FileAccess.Write))
                    {
                        await request.file.CopyToAsync(stream);
                    }

                    if (!string.IsNullOrWhiteSpace(entity.ImagePath))
                    {
                        System.IO.File.Delete(Path.Combine(env.ContentRootPath,
                                                          "wwwroot",
                                                          "uploads",
                                                          "files",
                                                          entity.ImagePath));
                    }

                    entity.ImagePath = request.ImagePath;

                }

                await db.SaveChangesAsync();
                return entity.Id;



            }
        }
    }
}
