using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Krbprojects.WebUI.Models.DbContexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Krbprojects.WebUI.AppCode.Providers
{
    public class AppClaimProvider : IClaimsTransformation
    {
        readonly KrbProjectsBaseDbContext db;
        public AppClaimProvider(KrbProjectsBaseDbContext db)
        {
            this.db = db;
        }
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity.IsAuthenticated && principal.Identity is ClaimsIdentity currentIdentity)
            {
                var userid = Convert.ToInt32(currentIdentity.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value);
                var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userid);

                #region userlere rollarin sehife yenilendikce verilmesi ve alinmasi real taymda
                var role = currentIdentity.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Role));
                while (role != null)
                {
                    currentIdentity.RemoveClaim(role);
                    role = currentIdentity.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Role));
                }
                var currentroles = (from ur in db.UserRoles
                                    join r in db.Roles on ur.RoleId equals r.Id
                                    where ur.UserId == userid
                                    select r.Name).ToArray();
                foreach (var rolename in currentroles)
                {
                    currentIdentity.AddClaim(new Claim(ClaimTypes.Role, rolename));
                }
                #endregion
                #region user rollara selahiyetlerin erilmesi real taymda
                var currentclaims = currentIdentity.Claims.Where(c => Program.principals.Contains(c.Type))
                    .ToArray();
                foreach (var claim in currentclaims)
                {
                    currentIdentity.RemoveClaim(claim);
                }
                var currentpolicies = await (from uc in db.UserClaims
                                             where uc.UserId == userid && uc.ClaimValue == "1"
                                             select uc.ClaimType)
                                          .Union(from rc in db.RoleClaims
                                                 join ur in db.UserRoles on rc.RoleId equals ur.RoleId
                                                 where ur.UserId == userid && rc.ClaimValue == "1"
                                                 select rc.ClaimType).ToListAsync();
                foreach (var policy in currentpolicies)
                {
                    currentIdentity.AddClaim(new Claim(policy, "1"));
                }
                #endregion
            }
            return principal;
        }

    }
}
