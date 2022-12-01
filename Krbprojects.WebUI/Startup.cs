using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Krbprojects.WebUI.AppCode.Providers;
using Krbprojects.WebUI.Models.DbContexts;
using Krbprojects.WebUI.Models.Entities.Membership;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Krbprojects.WebUI
{
    public class Startup
    {
        readonly IConfiguration configuration;
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(cfg => {
                cfg.ModelBinderProviders.Insert(0, new BooleanBinderProvider());
                var policy = new AuthorizationPolicyBuilder()
                       .RequireAuthenticatedUser()
                       .Build();

                cfg.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddDbContext<KrbProjectsBaseDbContext>(cfg =>
            {
                cfg.UseSqlServer(configuration.GetConnectionString("cstring"));
            })
                .AddIdentity<KrbUser, KrbRole>()
                .AddEntityFrameworkStores<KrbProjectsBaseDbContext>();
            services.Configure<IdentityOptions>(cfg =>
            {
                cfg.Password.RequireDigit = false;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequiredUniqueChars = 1;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequiredLength = 3;

                cfg.User.RequireUniqueEmail = true;

                cfg.Lockout.MaxFailedAccessAttempts = 3;
                //3
                cfg.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0, 10, 0);

                cfg.SignIn.RequireConfirmedEmail = true;
            });
            services.ConfigureApplicationCookie(cfg =>
            {
                cfg.LoginPath = "/signin.html";
                cfg.AccessDeniedPath = "/accesdenied.html";
                cfg.ExpireTimeSpan = new TimeSpan(0, 5, 0);
                cfg.Cookie.Name = "krb";
            });
            services.AddAuthentication();
            services.AddAuthorization(cfg =>
            {
                foreach (var policyname in Program.principals)
                {
                    cfg.AddPolicy(policyname, p =>
                    {
                        p.RequireAssertion(h =>
                        {
                            return h.User.IsInRole("SuperAdmin") ||
                             h.User.HasClaim(policyname, "1");
                        });
                    });
                }


            });
            services.AddScoped<UserManager<KrbUser>>();
            services.AddScoped<SignInManager<KrbUser>>();
            services.AddScoped<RoleManager<KrbRole>>();
           
            
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddMediatR(Assembly.GetExecutingAssembly());

            
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.SeedMembership();
            app.UseStaticFiles();
           
            app.UseRouting();
            app.UseRequestLocalization(cfg =>
            {
                cfg.AddSupportedUICultures("az","en","ru");

                cfg.AddSupportedCultures("az", "en", "ru");

                cfg.RequestCultureProviders.Clear();
                cfg.RequestCultureProviders.Add(new CultureProvider());
            });

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //    name: "default-sigin",
                //    pattern: "signin.html",
                //    defaults:new
                //    {
                //        area="KrbAdmin",
                //        controller="account",
                //        action="signin"
                //    });
                //endpoints.MapControllerRoute(
                //    name: "default-register",
                //    pattern: "register.html",
                //    defaults:new
                //    {
                //        area="KrbAdmin",
                //        controller="account",
                //        action="register"
                //    });

                endpoints.MapControllerRoute(
                    name: "default-accesdenied",
                    pattern: "accesdenied.html",
                    defaults: new
                    {
                        area = "KrbAdmin",
                        controller = "account",
                        action = "accesdeny"
                    });
                endpoints.MapControllerRoute(
                name: "areas-with-lang",
                pattern: "{lang}/{area:exists}/{controller=Home}/{action=Index}/{id?}",
                constraints: new
                {
                    lang="az|en|ru"
                }
                );

                endpoints.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                endpoints.MapControllerRoute("default", "{lang}/{controller=Dashboard}/{action=index}/{id?}",
                    constraints: new
                    {
                        lang = "az|en|ru"
                    });

                endpoints.MapControllerRoute("default", "{controller=Dashboard}/{action=index}/{id?}");

            });
        }
    }
}
