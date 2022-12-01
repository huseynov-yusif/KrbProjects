using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Krbprojects.WebUI.Areas.KrbAdmin.Models.ViewModels;
using Krbprojects.WebUI.Models.DbContexts;
using Krbprojects.WebUI.Modules.ContactPagePhotoModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Krbprojects.WebUI.Areas.KrbAdmin.Controllers
{
    [Area("KrbAdmin")]
    public class ContactPagePhotoCRUDController : Controller
    {
        //teksekilli
        readonly KrbProjectsBaseDbContext db;
        readonly IMediator mediator;


        public ContactPagePhotoCRUDController(KrbProjectsBaseDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }
        [Authorize(Policy = "admin.contactpagephotocrud.index")]
        public async Task<IActionResult> Index()
        {
            var photo = await db.ContactPagePhotos.ToListAsync();

            return View(photo);
        }
        [Authorize(Policy = "admin.contactpagephotocrud.delete")]

        public async Task<IActionResult> Delete(int id)
        {
            var photo = await db.ContactPagePhotos.FirstOrDefaultAsync(a => a.Id == id);
            return View(photo);
        }
        [Authorize(Policy = "admin.contactpagephotocrud.delete")]

        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var photo = await db.ContactPagePhotos.FirstOrDefaultAsync(a => a.Id == id);
            db.Remove(photo);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }





        [HttpGet]
        [Authorize(Policy = "admin.contactpagephotocrud.create")]

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Policy = "admin.contactpagephotocrud.create")]

        public async Task<IActionResult> Create(ContactPagePhotoCreateCommand info)
        {
            var response = await mediator.Send(info);
            return Redirect(nameof(Index));

        }

        [HttpGet]
        [Authorize(Policy = "admin.contactpagephotocrud.edit")]

        public async Task<IActionResult> Edit(int id)
        {
            var photo = await db.ContactPagePhotos.FirstOrDefaultAsync(a => a.Id == id);
            var model = new ContactPagePhotoViewModel();
            model.Id = photo.Id;
            model.Name = photo.Name;
            model.ImagePath = photo.ImagePath;
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "admin.contactpagephotocrud.edit")]

        public async Task<IActionResult> Edit(ContactPagePhotoUpdateCommand information)
        {

            var response = await mediator.Send(information);

            return RedirectToAction(nameof(Index));
        }
    }
}
