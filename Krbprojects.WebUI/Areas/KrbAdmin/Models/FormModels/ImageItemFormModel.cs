using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Krbprojects.WebUI.Areas.KrbAdmin.Models
{
    public class ImageItemFormModel
    {
       
            public int? Id { get; set; }
            public bool IsMain { get; set; }
            public string TempPath { get; set; }
            public IFormFile File { get; set; }
        
    }
}
