using System.Linq;
using System.Security.Claims;

namespace Krbprojects.WebUI.AppCode.Extensions
{
    static public partial class Extension
    {
        public static string[] principals = null;

        static public string GetPrincipalName(this ClaimsPrincipal principal)
        {
            string name = principal.Claims.FirstOrDefault(c => c.Type.Equals("Name"))?.Value;

            if (!string.IsNullOrWhiteSpace(name))
            {
                return $"{name}";
            }

            return principal.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))?.Value;
        }

        static public bool HasAccess(this ClaimsPrincipal principal, string policyName)
        {
            return principal.IsInRole("SuperAdmin") ||
                principal.HasClaim(c => c.Type.Equals(policyName) && c.Value.Equals("1"));
        }
    }
}
