using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Krbprojects.WebUI.Models.Entities
{
    public class Technique:BaseEntity
    {
        public string Name_AZ { get; set; }
        public string Name_EN { get; set; }
        public string Name_RU { get; set; }
        public string ImagePath { get; set; }
    }
}
