using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Krbprojects.WebUI.Areas.KrbAdmin.Models.ViewModels
{
    public class ContactPagePhotoViewModel
    {
        public int? Id { get; set; }
        public string Name{ get; set; }
        public string ImagePath { get; set; }
        public IFormFile file { get; set; }
        public string fileTemp { get; set; }
    }
}
