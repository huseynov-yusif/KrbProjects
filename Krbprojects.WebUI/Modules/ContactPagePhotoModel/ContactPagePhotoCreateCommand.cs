using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Krbprojects.WebUI.Models.DbContexts;
using Krbprojects.WebUI.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Krbprojects.WebUI.Modules.ContactPagePhotoModel
{
    public class ContactPagePhotoCreateCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public IFormFile file { get; set; }
        public class ContactPagePhotoCreateCommandHandler : IRequestHandler<ContactPagePhotoCreateCommand, int>
        {
            readonly KrbProjectsBaseDbContext db;
            readonly IActionContextAccessor ctx;
            readonly IWebHostEnvironment env;
            public ContactPagePhotoCreateCommandHandler(KrbProjectsBaseDbContext db, IActionContextAccessor ctx, IWebHostEnvironment env)
            {
                this.ctx = ctx;
                this.db = db;
                this.env = env;
            }
            public async Task<int> Handle(ContactPagePhotoCreateCommand request, CancellationToken cancellationToken)
            {
                if (request.file == null)
                {
                    ctx.ActionContext.ModelState.AddModelError("file", "There is not image");
                }


                var model = new ContactPagePhoto();
                model.ImagePath = request.ImagePath;
                model.Name = request.Name;

                string extension = Path.GetExtension(request.file.FileName);
                model.ImagePath = $"{Guid.NewGuid()}{extension}";

                string physicalFileName = Path.Combine(env.ContentRootPath,
                                                       "wwwroot",
                                                       "uploads",
                                                       "files",
                                                       model.ImagePath);

                using (var stream = new FileStream(physicalFileName, FileMode.Create, FileAccess.Write))
                {
                    await request.file.CopyToAsync(stream);
                }

                db.ContactPagePhotos.Add(model);
                await db.SaveChangesAsync();

                return model.Id;


            }
        }
    }
}
