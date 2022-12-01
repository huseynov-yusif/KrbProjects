using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Krbprojects.WebUI.AppCode.Extensions;
using Krbprojects.WebUI.Areas.KrbAdmin.Models.FormModels;
using Krbprojects.WebUI.Models.Entities.Membership;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Krbprojects.WebUI.Areas.KrbAdmin.Controllers
{
    [Area("KrbAdmin")]
    public class AccountController : Controller
    {
        readonly SignInManager<KrbUser> signInManager;
        readonly UserManager<KrbUser> userManager;
        readonly IConfiguration configuration;
        public AccountController(SignInManager<KrbUser> signInManager, UserManager<KrbUser> userManager, IConfiguration configuration)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.configuration = configuration;
        }
        [AllowAnonymous]
        [Route("/SignIn.html")]

        public IActionResult SignIn()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("/SignIn.html")]

        public async Task<IActionResult> SignIn(LoginFormModel user)
        {
            if (ModelState.IsValid)
            {
                KrbUser foundedUser = null;
                if (user.UserName.IsEmail())
                {
                    foundedUser = await userManager.FindByEmailAsync(user.UserName);
                }
                else
                {
                    foundedUser = await userManager.FindByNameAsync(user.UserName);

                }
                if (foundedUser == null)
                {

                    ViewBag.Massage = ("İstifadəçi adınız və ya şifrəniz yalnışdır");
                    goto end;
                }
                var signnResult = await signInManager.PasswordSignInAsync(foundedUser, user.Password, true, true);
                if (!signnResult.Succeeded)
                {
                    ViewBag.Massage = ("İstifadəçi adınız və ya şifrəniz yalnışdır");
                    goto end;
                }
                var callbackUrl = Request.Query["ReturnUrl"];
                if (!string.IsNullOrWhiteSpace(callbackUrl))
                {
                    return RedirectToPage(callbackUrl);
                }

                return RedirectToAction("index", "home");
            }
        end:
            return View(user);
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("/Register.html")]
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("/Register.html")]

        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new KrbUser();
                user.Email = model.Email;
                user.UserName = model.Email;
                user.Name = model.Name;
                user.SurName = model.SurName;
                user.EmailConfirmed = true;
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    ViewBag.Massage = "Qeydiyyat tamamlandi";
                    return RedirectToAction(nameof(SignIn));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

            }
            return View(model);
        }
        [Route("/logout.html")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("signin", "account");
        }
        [AllowAnonymous]
        [Route("/accessdenied.html")]
        public IActionResult AccessDeny()
        {
            return View();
        }
    }

}
