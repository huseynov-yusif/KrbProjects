using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Krbprojects.WebUI.Areas.KrbAdmin.Models;

namespace Krbprojects.WebUI.Models.Entities
{
    public class Worked
    {
        //bu entityde indiye qeder tehfil verilmis layiheler saxlanilir
        public long Id { get; set; }
        public long? CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public long? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string Name_AZ { get; set; }
        public string Name_EN { get; set; }
        public string Name_RU { get; set; }
        
        public string Description_AZ { get; set; }
        public string Description_EN { get; set; }
        public string Description_RU { get; set; }
        public virtual ICollection<WorkedImage> Images { get; set; }

        [NotMapped] //database'a dusmesin
        public virtual ImageItemFormModel[] Files { get; set; }
    }
}
