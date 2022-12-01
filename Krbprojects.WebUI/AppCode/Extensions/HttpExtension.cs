using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Krbprojects.WebUI.AppCode.Extensions
{
    static public partial class Extension
    {
        public static string GetCurrentCulture(this HttpContext ctx)
        {
            var match = Regex.Match(ctx.Request.Path, @"\/(?<lang>az|en|ru)\/?.*");

            if (match.Success)
            {
                return match.Groups["lang"].Value;
            }

            if (ctx.Request.Cookies.TryGetValue("lang", out string lang))
            {
                return lang;
            }

            return "az";
        }
    }
}
