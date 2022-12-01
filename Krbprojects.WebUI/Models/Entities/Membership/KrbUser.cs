using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Krbprojects.WebUI.Models.Entities.Membership
{
    public class KrbUser:IdentityUser<int>
    {
        [Required]
        public string Name { get; set; }
        [Required]

        public string SurName { get; set; }
    }
}
