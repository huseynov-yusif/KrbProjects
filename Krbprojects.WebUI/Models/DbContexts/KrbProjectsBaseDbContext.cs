using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Krbprojects.WebUI.Models.Entities;
using Krbprojects.WebUI.Models.Entities.Membership;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Krbprojects.WebUI.Models.DbContexts
{
    public class KrbProjectsBaseDbContext:IdentityDbContext<KrbUser,KrbRole,int,KrbUserClaim,KrbUserRole,KrbUserLogin,KrbRoleClaim,KrbUserToken>
    {
        public KrbProjectsBaseDbContext()
           : base()
        {

        }
        public KrbProjectsBaseDbContext(DbContextOptions options)
            : base(options)
        {

        }
        

        public DbSet<AboutUs> AboutUses { get; set; }
        public DbSet<Information> Informations { get; set; }
        public DbSet<Technique> Techniques { get; set; }
        public DbSet<WhatWeDo> WhatWeDoes { get; set; }
        public DbSet<ContactPagePhoto> ContactPagePhotos { get; set; }
        public DbSet<ContactPost> ContactPosts { get; set; }
        public DbSet<Worked> Workeds { get; set; }
        public DbSet<WorkedImage> WorkedImages { get; set; }
        public DbSet<HomePage> HomePages { get; set; }
        public DbSet<HomePageImage> HomePageImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<KrbUser>(e => {

                e.ToTable("Users", "Membership");
            });

            builder.Entity<KrbRole>(e => {

                e.ToTable("Roles", "Membership");
            });

            builder.Entity<KrbRoleClaim>(e => {

                e.ToTable("RoleClaims", "Membership");
            });

            builder.Entity<KrbUserClaim>(e => {

                e.ToTable("UserClaims", "Membership");
            });

            builder.Entity<KrbUserLogin>(e => {

                e.ToTable("UserLogins", "Membership");
            });

            builder.Entity<KrbUserToken>(e => {

                e.ToTable("UserTokens", "Membership");
            });

            builder.Entity<KrbUserRole>(e => {

                e.ToTable("UserRoles", "Membership");
            });

        }
    }
}
