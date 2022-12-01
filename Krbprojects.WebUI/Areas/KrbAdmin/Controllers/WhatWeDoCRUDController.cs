using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Krbprojects.WebUI.Areas.KrbAdmin.Models.ViewModels;
using Krbprojects.WebUI.Models.DbContexts;
using Krbprojects.WebUI.Modules.WhatWeDoModul;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Krbprojects.WebUI.Areas.KrbAdmin.Controllers
{
    [Area("KrbAdmin")]
    public class WhatWeDoCRUDController : Controller
    {
        readonly KrbProjectsBaseDbContext db;
        readonly IMediator mediator;
        public WhatWeDoCRUDController(KrbProjectsBaseDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }
        [Authorize(Policy = "admin.whatwedocrud.index")]

        public async Task<IActionResult> Index()
        {
            var work = await db.WhatWeDoes.ToListAsync();
            return View(work);
        }
        [Authorize(Policy = "admin.whatwedocrud.delete")]

        public async Task<IActionResult> Delete(int id)
        {
            var work = await db.WhatWeDoes.FirstOrDefaultAsync(a => a.Id == id);
            return View(work);
        }
        [Authorize(Policy = "admin.whatwedocrud.delete")]

        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var work = await db.WhatWeDoes.FirstOrDefaultAsync(a => a.Id == id);
            db.Remove(work);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Policy = "admin.whatwedocrud.create")]

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Policy = "admin.whatwedocrud.create")]

        public async Task<IActionResult> Create(WhatWeDoCreateCommand work)
        {
            var response = await mediator.Send(work);
            return Redirect(nameof(Index));

        }
        [HttpGet]
        [Authorize(Policy = "admin.whatwedocrud.edit")]

        public async Task<IActionResult> Edit(int id)
        {
            var work = await db.WhatWeDoes.FirstOrDefaultAsync(a => a.Id == id);
            var model = new WhatWeDoViewModel();
            model.Id = work.Id;
            model.Name_AZ = work.Name_AZ;
            model.Name_EN = work.Name_EN;
            model.Name_RU = work.Name_RU;
            model.Description_AZ = work.Description_AZ;
            model.Description_EN = work.Description_EN;
            model.Description_RU = work.Description_RU;
            model.ImagePath = work.ImagePath;
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "admin.whatwedocrud.edit")]

        public async Task<IActionResult> Edit(WhatWeDoUpdateCommand work)
        {

            var response = await mediator.Send(work);

            return RedirectToAction(nameof(Index));
        }
    }
}
