using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Krbprojects.WebUI.Models.Entities
{
    public class WorkedImage
    {
        public long Id { get; set; }
        public long? CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public long? DeletedByUserId { get; set; }
        public string FileName { get; set; }
        public long WorkedId { get; set; }
        public bool IsMain { get; set; }
        public virtual Worked Worked { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
