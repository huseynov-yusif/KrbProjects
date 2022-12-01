using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Krbprojects.WebUI.Models.DbContexts;
using Krbprojects.WebUI.Models.Entities.Membership;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Krbprojects.WebUI.Areas.KrbAdmin.Controllers
{
    [Area("KrbAdmin")]
    public class UsersController : Controller
    {
        readonly KrbProjectsBaseDbContext db;
        public UsersController(KrbProjectsBaseDbContext db)
        {
            this.db = db;
        }
        [Authorize(Policy = "admin.users.index")]
        public async Task<IActionResult> Index()
        {
            var data = await db.Users.ToListAsync();
            return View(data);
        }
        [Authorize(Policy = "admin.users.details")]
        public async Task<IActionResult> Details(int id)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            ViewBag.Roles = await (from r in db.Roles
                                   join ur in db.UserRoles
                                   on new { RoleId = r.Id, UserId = user.Id } equals new { ur.RoleId, ur.UserId } into lJoin
                                   from lj in lJoin.DefaultIfEmpty()
                                   select Tuple.Create(r.Id, r.Name, lj != null)).ToListAsync();
            ViewBag.Principals = (from p in Program.principals
                                  join uc in db.UserClaims on new { ClaimValue = "1", ClaimType = p, UserId = user.Id } equals new { uc.ClaimValue, uc.ClaimType, uc.UserId } into lJoin
                                  from lj in lJoin.DefaultIfEmpty()
                                  select Tuple.Create(p, lj != null)).ToList();
            return View(user);
        }
        [HttpPost]
        [Authorize(Policy = "admin.users.setrole")]
        [Route("/user-set-role")]
        public async Task<IActionResult> SetRole(int userId, int roleId, bool selected)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var role = await db.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
            if (user == null)
            {
                return Json(new
                {
                    error = true,
                    massage = "Xətali sorğu"
                });
            }

            

            if (role == null)
            {
                return Json(new
                {
                    error = true,
                    massage = "Xətali sorğu"
                });
            }
            if (selected)
            {
                if (await db.UserRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId))
                {
                    return Json(new
                    {
                        error = true,
                        massage = $"'{user.Name}{user.UserName}' adlı istifadəçi'{role.Name}' adlı roldadır"
                    });
                }
                else
                {
                    db.UserRoles.Add(new KrbUserRole
                    {
                        UserId = userId,
                        RoleId = roleId
                    });
                    await db.SaveChangesAsync();
                    return Json(new
                    {
                        error = false,
                        massage = $"'{user.Name}{user.SurName}' adlı istifadəçi'{role.Name}' rola əlavə edildi"
                    });
                }
            }
            else
            {
                var userRole = await db.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
                if (userRole == null)
                {
                    return Json(new
                    {
                        error = true,
                        massage = $"'{user.Name}{user.SurName}' adlı istifadəçi'{role.Name}' adlı rolda deyil"
                    });
                }
                else
                {
                    db.UserRoles.Remove(userRole);
                    await db.SaveChangesAsync();
                    return Json(new
                    {
                        error = false,
                        massage = $"'{user.Name}{user.SurName}' adlı istifadəçi '{role.Name}'adlı roldan çıxarıldı"
                    });
                }
            }
        }

        [HttpPost]
        [Authorize(Policy = "admin.users.setprincipals")]

        [Route("/user-set-pricipals")]
        public async Task<IActionResult> SetPrincipals(int userId, string principalName, bool selected)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var hasPrincipal = Program.principals.Contains(principalName);
            if (user == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Xətalı sorğu"
                });
            }
           
            if (!hasPrincipal)
            {
                return Json(new
                {
                    error = true,
                    message = "Xətalı sorğu"
                });
            }
            if (selected)
            {
                if (await db.UserClaims.AnyAsync(uc => uc.UserId == userId && uc.ClaimType.Equals(principalName) && uc.ClaimValue.Equals("1")))
                {
                    return Json(new
                    {
                        error = true,
                        message = $"'{user.Name}{user.SurName}' adlı istifadəçi'{principalName}'adlı səlahiyətə malikdir"
                    });
                }
                else
                {
                    db.UserClaims.Add(new KrbUserClaim
                    {
                        UserId = userId,
                        ClaimType = principalName,
                        ClaimValue = "1"
                    });
                    await db.SaveChangesAsync();
                    return Json(new
                    {
                        error = false,
                        message = $"'{principalName}' adlı səlahiyyət '{user.Name}{user.SurName}' adlı istifadəçiyə verildi"
                    });
                }
            }
            else
            {
                var userClaim = await db.UserClaims.FirstOrDefaultAsync(uc => uc.UserId == userId && uc.ClaimType.Equals(principalName) && uc.ClaimValue.Equals("1"));
                if (userClaim == null)
                {
                    return Json(new
                    {
                        error = true,
                        message = $"'{user.Name}{user.SurName}' adli istifadəçi '{principalName}' adlı səlahiyyətə malik deyil"
                    });
                }
                else
                {
                    db.UserClaims.Remove(userClaim);
                    await db.SaveChangesAsync();
                    return Json(new
                    {
                        error = false,
                        message = $"'{user.Name}{user.SurName}' adlı istifadəçidən '{principalName}' adlı səlahiyət alındı"
                    });
                }
            }
        }
    }
}
