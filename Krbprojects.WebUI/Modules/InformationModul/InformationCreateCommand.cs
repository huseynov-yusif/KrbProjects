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

namespace Krbprojects.WebUI.Modules.InformationModul
{
    public class InformationCreateCommand : IRequest<int>
    {
        public string Name_AZ { get; set; }
        public string Name_EN { get; set; }
        public string Name_RU { get; set; }
        public string Description_AZ { get; set; }
        public string Description_EN { get; set; }
        public string Description_RU { get; set; }
        public string ImagePath { get; set; }
        public IFormFile file { get; set; }
        public class InformationCreateCommandHandler : IRequestHandler<InformationCreateCommand, int>
        {
            readonly KrbProjectsBaseDbContext db;
            readonly IActionContextAccessor ctx;
            readonly IWebHostEnvironment env;
            public InformationCreateCommandHandler(KrbProjectsBaseDbContext db, IActionContextAccessor ctx, IWebHostEnvironment env)
            {
                this.ctx = ctx;
                this.db = db;
                this.env = env;
            }
            public async Task<int> Handle(InformationCreateCommand request, CancellationToken cancellationToken)
            {
                if (request.file == null)
                {
                    ctx.ActionContext.ModelState.AddModelError("file", "There is not image");
                }


                var model = new Information();
                model.ImagePath = request.ImagePath;
                model.Name_AZ = request.Name_AZ;
                model.Name_EN = request.Name_EN;
                model.Name_RU = request.Name_RU;
                model.Description_AZ = request.Description_AZ;
                model.Description_EN = request.Description_EN;
                model.Description_RU = request.Description_RU;

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

                db.Informations.Add(model);
                await db.SaveChangesAsync();

                return model.Id;


            }
        }
    }
}
