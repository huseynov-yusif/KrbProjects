
using System.Threading.Tasks;
using Krbprojects.WebUI.Areas.KrbAdmin.Models.ViewModels;
using Krbprojects.WebUI.Models.DbContexts;
using Krbprojects.WebUI.Modules.InformationModul;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Krbprojects.WebUI.Areas.KrbAdmin.Controllers
{
    [Area("KrbAdmin")]
    public class InformationCRUDController : Controller
    {
        //teksekilli
        readonly KrbProjectsBaseDbContext db;
        readonly IMediator mediator;


        public InformationCRUDController(KrbProjectsBaseDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }
        [Authorize(Policy = "admin.informationcrud.index")]

        public async Task<IActionResult> Index()
        {
            var information = await db.Informations.ToListAsync();

            return View(information);
        }
        [Authorize(Policy = "admin.informationcrud.delete")]

        public async Task<IActionResult> Delete(int id)
        {
            var information = await db.Informations.FirstOrDefaultAsync(a=>a.Id==id);
            return View(information);
        }
        [Authorize(Policy = "admin.informationcrud.delete")]

        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var information = await db.Informations.FirstOrDefaultAsync(a=>a.Id == id);
            db.Remove(information);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }





        [HttpGet]
        [Authorize(Policy = "admin.informationcrud.create")]

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Policy = "admin.informationcrud.create")]

        public async Task<IActionResult> Create(InformationCreateCommand info)
        {
            var response = await mediator.Send(info);
            return Redirect(nameof(Index));

        }

        [HttpGet]
        [Authorize(Policy = "admin.informationcrud.edit")]

        public async Task<IActionResult> Edit(int id)
        {
            var information = await db.Informations.FirstOrDefaultAsync(a => a.Id == id);
            var model = new InformationViewModel();
            model.Id = information.Id;
            model.Name_AZ = information.Name_AZ;
            model.Name_EN = information.Name_EN;
            model.Name_RU = information.Name_RU;
            model.Description_AZ = information.Description_AZ;
            model.Description_EN = information.Description_EN;
            model.Description_RU = information.Description_RU;
            model.ImagePath = information.ImagePath;
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "admin.informationcrud.edit")]

        public async Task<IActionResult> Edit(InformationUpdateCommand information)
        {

            var response = await mediator.Send(information);

            return RedirectToAction(nameof(Index));
        }
    }
}
