using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Krbprojects.WebUI.Areas.KrbAdmin.Models.ViewModels;
using Krbprojects.WebUI.Models.DbContexts;
using Krbprojects.WebUI.Modules.HomePageModul;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Krbprojects.WebUI.Areas.KrbAdmin.Controllers
{
    //esas sehifede olan karuselin sekillerine deyisiklik etmek ucun 
    [Area("KrbAdmin")]
    public class HomePageCRUDController : Controller
    {
        readonly KrbProjectsBaseDbContext db;
        readonly IMediator mediator;
        public HomePageCRUDController(KrbProjectsBaseDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }
        [Authorize(Policy = "admin.HomePageCRUD.index")]

        public async Task<IActionResult> Index()
        {
            var home = await db.HomePages.ToListAsync();
            return View(home);
        }
        [HttpGet]
        [Authorize(Policy = "admin.HomePageCRUD.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var home = await db.HomePages.FirstOrDefaultAsync(h=>h.Id==id);
            return View(home);
        }
        [HttpPost]
        [Authorize(Policy = "admin.HomePageCRUD.Delete")]
        public async Task<IActionResult> DeleteConfirm(long id)
        {
            var home = await db.HomePages.Include(h=>h.Images).FirstOrDefaultAsync(h => h.Id == id);
            db.Remove(home);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Policy = "admin.HomePageCRUD.Create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.HomePageCRUD.Create")]
        public async Task<IActionResult> Create(HomePageCreateCommand command)
        {
            var response = await mediator.Send(command);


            if (response > 0)
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(Create));
        }
        [Authorize(Policy = "admin.HomePageCRUD.Edit")]
        public async Task<IActionResult> Edit(HomePageSingleQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }

            var vm = new HomePageViewModel();
            vm.Id = response.Id;
            vm.Name = response.Name;
            vm.Images = response.Images;
            vm.Files = response.Files;
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.HomePageCRUD.Edit")]
        public async Task<IActionResult> Edit(HomePageUpdateCommand command)
        {
            var response = await mediator.Send(command);

            if (response > 0)
                return RedirectToAction(nameof(Index));

            return View(command);

        }

    }
}
