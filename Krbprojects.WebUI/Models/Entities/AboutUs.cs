using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Krbprojects.WebUI.Models.Entities
{
    public class AboutUs:BaseEntity
    {
        public string Description_AZ { get; set; }
        public string Description_EN { get; set; }
        public string Description_RU { get; set; }
        public string ImagePath { get; set; }
    }
}
