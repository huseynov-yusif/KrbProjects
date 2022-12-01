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

namespace Krbprojects.WebUI.Modules.HomePageModul
{
    public class HomePageCreateCommand : IRequest<long>
    {
        [Required]
        public string Name { get; set; }
        public ICollection<HomePageImage> Images { get; set; }
        public ImageItemFormModel[] images { get; set; }

        public class HomePageCreateCommandHandler : IRequestHandler<HomePageCreateCommand, long>
        {
            readonly KrbProjectsBaseDbContext db;
            readonly IActionContextAccessor ctx;
            readonly IWebHostEnvironment env;
            readonly IMapper mapper;
            public HomePageCreateCommandHandler(KrbProjectsBaseDbContext db, IActionContextAccessor ctx, IWebHostEnvironment env, IMapper mapper)
            {
                this.env = env;
                this.ctx = ctx;
                this.db = db;
                this.mapper = mapper;
            }
            public async Task<long> Handle(HomePageCreateCommand request, CancellationToken cancellationToken)
            {
                if (request.images == null || !request.images.Any(i => i.File != null))
                {
                    ctx.ActionContext.ModelState.AddModelError("Images", "There are not images");
                }

                if (ctx.IsModelStateValid())
                {

                    HomePage home = new HomePage();
                    home.Name = request.Name;
                    home.Images = request.Images;

                    home.Images = new List<HomePageImage>();
                    foreach (var image in request.images.Where(i => i.File != null))
                    {
                        string extension = Path.GetExtension(image.File.FileName); //.jpg
                        string imagePath = $"{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid()}{extension}";
                        string physicalPath = Path.Combine(env.ContentRootPath,
                            "wwwroot",
                            "uploads",
                            "homepagephotos",
                            imagePath);

                        using (var stream = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                        {
                            await image.File.CopyToAsync(stream);
                        }

                        home.Images.Add(new HomePageImage
                        {
                            IsMain = image.IsMain,
                            FileName = imagePath
                        });
                    }

                    db.HomePages.Add(home);
                    await db.SaveChangesAsync(cancellationToken);



                    return home.Id;
                }
                return 0;


            }
        }
    }
}
