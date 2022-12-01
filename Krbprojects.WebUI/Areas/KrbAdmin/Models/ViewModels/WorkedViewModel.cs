
using System.Collections.Generic;
using Krbprojects.WebUI.Models.Entities;

namespace Krbprojects.WebUI.Areas.KrbAdmin.Models.ViewModels
{
    public class WorkedViewModel
    {
        public long? Id { get; set; }
        public string Name_AZ { get; set; }
        public string Name_EN { get; set; }
        public string Name_RU { get; set; }
       
        public string Description_AZ { get; set; }
        public string Description_EN { get; set; }
        public string Description_RU { get; set; }
        public ICollection<WorkedImage> Images { get; set; }
        public ImageItemFormModel[] Files { get; set; }
    }
}
