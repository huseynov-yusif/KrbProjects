using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Krbprojects.WebUI.Areas.KrbAdmin.Models;

namespace Krbprojects.WebUI.Models.Entities
{
    public class HomePage
    {
        //bu entity onun ucundurki esas sehifenin sekillerini multipleimager yaza bilek 
        //ad istifade olunmur ona gorediki esas sehifenin sekillerine deyisiklikler edile bilsin meselcun karuselin esas seklini deyismek elave elemek yaxud yenisini elave etmek 
        public long Id { get; set; }
        public long? CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public long? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string Name { get; set; }
        public virtual ICollection<HomePageImage> Images { get; set; }

        [NotMapped] //database'a dusmesin
        public virtual ImageItemFormModel[] Files { get; set; }
    }
}
