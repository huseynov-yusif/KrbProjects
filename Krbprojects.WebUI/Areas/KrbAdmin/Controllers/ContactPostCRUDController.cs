using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Krbprojects.WebUI.Models.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Krbprojects.WebUI.Areas.KrbAdmin.Controllers
{
    [Area("KrbAdmin")]
    public class ContactPostCRUDController : Controller
    {
        readonly KrbProjectsBaseDbContext db;
        public ContactPostCRUDController(KrbProjectsBaseDbContext db)
        {
            this.db = db;
        }
        [Authorize(Policy = "admin.ContactPostCRUD.index")]

        public async Task<IActionResult> Index()
        {
            var post = await db.ContactPosts.ToListAsync();
            return View(post);
        }
        [Authorize(Policy = "admin.ContactPostCRUD.details")]

        public async Task<IActionResult> Details(int id)
        {
            var post = await db.ContactPosts.FirstOrDefaultAsync(p => p.Id == id);
            return View(post);
        }
        [Authorize(Policy = "admin.ContactPostCRUD.delete")]

        public async Task<IActionResult> Delete(int id)
        {
            var post = await db.ContactPosts.FirstOrDefaultAsync(p => p.Id == id);
            return View(post);
        }
        [Authorize(Policy = "admin.ContactPostCRUD.delete")]

        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var post = await db.ContactPosts.FirstOrDefaultAsync(p => p.Id == id);
            db.Remove(post);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
