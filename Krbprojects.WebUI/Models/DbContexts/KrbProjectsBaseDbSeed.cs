
using Krbprojects.WebUI.Models.Entities.Membership;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Krbprojects.WebUI.Models.DbContexts
{
    static public class KrbProjectsBaseDbSeed
    {
        static public IApplicationBuilder SeedMembership(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var role = new KrbRole
                {
                    Name = "SuperAdmin"
                };
                var rolemanager = scope.ServiceProvider.GetRequiredService<RoleManager<KrbRole>>();
                var usermanager = scope.ServiceProvider.GetRequiredService<UserManager<KrbUser>>();

                if (rolemanager.RoleExistsAsync(role.Name).Result)
                {
                    role = rolemanager.FindByNameAsync(role.Name).Result;
                }
                else
                {
                    var rolecreateresult = rolemanager.CreateAsync(role).Result;
                    if (!rolecreateresult.Succeeded)
                        goto end;
                }
                string pasword = "111111";
                var user = new KrbUser
                {
                    Name = "Yusif",
                    SurName = "Huseynov",
                    Email = "yusifhh@code.edu.az",
                    EmailConfirmed = true,
                    UserName = "Yusif"
                };
                var foundeduser = usermanager.FindByEmailAsync(user.Email).Result;
                if (foundeduser != null && !usermanager.IsInRoleAsync(foundeduser, role.Name).Result)
                {
                    usermanager.AddToRoleAsync(foundeduser, role.Name).Wait();
                }
                else if (foundeduser == null)
                {
                    var usercreateresult = usermanager.CreateAsync(user, pasword).Result;
                    if (!usercreateresult.Succeeded)
                        goto end;
                    usermanager.AddToRoleAsync(user, role.Name).Wait();
                }
            }
        end:
            return app;
        }

    }
}
