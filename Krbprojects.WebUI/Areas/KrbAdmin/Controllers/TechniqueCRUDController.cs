using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Krbprojects.WebUI.Areas.KrbAdmin.Models.ViewModels;
using Krbprojects.WebUI.Models.DbContexts;
using Krbprojects.WebUI.Modules.TechniqueModul;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Krbprojects.WebUI.Areas.KrbAdmin.Controllers
{
    [Area("KrbAdmin")]
    public class TechniqueCRUDController : Controller
    {
        readonly KrbProjectsBaseDbContext db;
        readonly IMediator mediator;
        public TechniqueCRUDController(KrbProjectsBaseDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }
        [Authorize(Policy = "admin.techniquecrud.index")]

        public async Task< IActionResult> Index()
        {
            var technique = await db.Techniques.ToListAsync();
            return View(technique);
        }
        [HttpGet]
        [Authorize(Policy = "admin.techniquecrud.delete")]

        public async Task<IActionResult> Delete(int id)
        {
            var technique = await db.Techniques.FirstOrDefaultAsync(a => a.Id == id);
            return View(technique);
        }
        [Authorize(Policy = "admin.techniquecrud.delete")]

        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var technique = await db.Techniques.FirstOrDefaultAsync(a => a.Id == id);
            db.Remove(technique);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Policy = "admin.techniquecrud.create")]

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Policy = "admin.techniquecrud.create")]

        public async Task<IActionResult> Create(TechniqueCreateCommand technique)
        {
            var response = await mediator.Send(technique);
            return Redirect(nameof(Index));

        }

        [HttpGet]
        [Authorize(Policy = "admin.techniquecrud.edit")]

        public async Task<IActionResult> Edit(int id)
        {
            var technique = await db.Techniques.FirstOrDefaultAsync(a => a.Id == id);
            var model = new TechniqueViewModel();
            model.Id = technique.Id;
            model.Name_AZ = technique.Name_AZ;
            model.Name_EN = technique.Name_EN;
            model.Name_RU = technique.Name_RU;
            model.ImagePath = technique.ImagePath;
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "admin.techniquecrud.edit")]

        public async Task<IActionResult> Edit(TechniqueUpdateCommand technique)
        {

            var response = await mediator.Send(technique);

            return RedirectToAction(nameof(Index));
        }

    }
}
