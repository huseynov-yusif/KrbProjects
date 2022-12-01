using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Krbprojects.WebUI.Models.Entities;

namespace Krbprojects.WebUI.Areas.KrbAdmin.Models.ViewModels
{
    public class HomePageViewModel
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public ICollection<HomePageImage> Images { get; set; }
        public ImageItemFormModel[] Files { get; set; }
    }
}
