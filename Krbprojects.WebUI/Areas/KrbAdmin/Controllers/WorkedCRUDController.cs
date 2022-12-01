using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Krbprojects.WebUI.Areas.KrbAdmin.Models.ViewModels;
using Krbprojects.WebUI.Models.DbContexts;
using Krbprojects.WebUI.Modules.WorkedModul;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Krbprojects.WebUI.Areas.KrbAdmin.Controllers
{
    [Area("KrbAdmin")]
    public class WorkedCRUDController : Controller
    {
        readonly KrbProjectsBaseDbContext db;
        readonly IMediator mediator;
        public WorkedCRUDController(KrbProjectsBaseDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }
        [Authorize(Policy = "admin.WorkedCRUD.Index")]
        public async Task<IActionResult> Index()
        {
            var work = await db.Workeds.Include(w => w.Images).ToListAsync();
            return View(work);
        }
        [Authorize(Policy = "admin.WorkedCRUD.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var work = await db.Workeds.FirstOrDefaultAsync(w=>w.Id==id);
            return View(work);
        }
        [Authorize(Policy = "admin.WorkedCRUD.Delete")]
        public async Task<IActionResult> DeleteConfirm(long id)
        {
            var work = await db.Workeds.Include(w => w.Images).FirstOrDefaultAsync(w=>w.Id==id);
            db.Remove(work);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Policy = "admin.WorkedCRUD.Create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.WorkedCRUD.Create")]
        public async Task<IActionResult> Create(WorkedCreateCommand command)
        {
            var response = await mediator.Send(command);


            if (response > 0)
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(Create));
        }
        [Authorize(Policy = "admin.WorkedCRUD.Edit")]
        public async Task<IActionResult> Edit(WorkedSingleQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }

            var vm = new WorkedViewModel();
            vm.Id = response.Id;
            vm.Name_AZ = response.Name_AZ;
            vm.Name_EN = response.Name_EN;
            vm.Name_RU = response.Name_RU;
            vm.Description_AZ = response.Description_AZ;
            vm.Description_EN = response.Description_EN;
            vm.Description_RU = response.Description_RU;
            vm.Images = response.Images;
            vm.Files = response.Files;
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.WorkedCRUD.Edit")]
        public async Task<IActionResult> Edit(WorkedUpdateCommand command)
        {
            var response = await mediator.Send(command);

            if (response > 0)
                return RedirectToAction(nameof(Index));

            return View(command);

        }
    }
    

}
