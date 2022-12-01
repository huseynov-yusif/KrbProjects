using System;
using Krbprojects.WebUI.Models.DbContexts;
using Krbprojects.WebUI.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Krbprojects.WebUI.Controllers
{
    [AllowAnonymous]
    public class ContactController : Controller
    {
        readonly KrbProjectsBaseDbContext db;
            public ContactController(KrbProjectsBaseDbContext db)
        {
            this.db = db;    
        }
        [HttpGet]
        public IActionResult Contact()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Contact(ContactPost model)
        {
            if (ModelState.IsValid)
            {
                db.ContactPosts.Add(model);
                db.SaveChanges();
                ModelState.Clear();
                ViewBag.Message = "Sizin sorğunuz qeydə alındı tezliklə sizə geri dönəcəyik!";
                return View();
            }
            return View(model);
        }
    }
} 
