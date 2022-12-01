using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Krbprojects.WebUI.Areas.KrbAdmin.Models.ViewModels;
using Krbprojects.WebUI.Models.DbContexts;
using Krbprojects.WebUI.Modules.AboutUsModul;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Krbprojects.WebUI.Areas.KrbAdmin.Controllers
{
    [Area("KrbAdmin")]
    public class AboutUsCRUDController : Controller
    {
        readonly KrbProjectsBaseDbContext db;
        readonly IMediator mediator;
        public AboutUsCRUDController(KrbProjectsBaseDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }
        [Authorize(Policy = "admin.aboutuscrud.index")]
        public async Task< IActionResult> Index()
        {
            var aboutus = await db.AboutUses.ToListAsync();
            return View(aboutus);
        }

        [Authorize(Policy = "admin.aboutuscrud.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var aboutus = await db.AboutUses.FirstOrDefaultAsync(a => a.Id == id);
            return View(aboutus);
        }

        [Authorize(Policy = "admin.aboutuscrud.delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var aboutus = await db.AboutUses.FirstOrDefaultAsync(a => a.Id == id);
            db.Remove(aboutus);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Policy = "admin.aboutuscrud.create")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Policy = "admin.aboutuscrud.create")]
        public async Task<IActionResult> Create(AboutUsCreateCommand aboutus)
        {
            var response = await mediator.Send(aboutus);
            return Redirect(nameof(Index));

        }
        [HttpGet]
        [Authorize(Policy = "admin.aboutuscrud.edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var aboutus = await db.AboutUses.FirstOrDefaultAsync(a => a.Id == id);
            var model = new AboutUsViewModel();
            model.Id = aboutus.Id;
            model.Description_AZ = aboutus.Description_AZ;
            model.Description_EN = aboutus.Description_EN;
            model.Description_RU = aboutus.Description_RU;
            model.ImagePath = aboutus.ImagePath;
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "admin.aboutuscrud.edit")]
        public async Task<IActionResult> Edit(AboutUsUpdateCommand aboutus)
        {

            var response = await mediator.Send(aboutus);

            return RedirectToAction(nameof(Index));
        }
    }
}
