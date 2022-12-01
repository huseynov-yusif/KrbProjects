using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Krbprojects.WebUI.Models.Entities
{
    public class ContactPost:BaseEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedByUserId { get; set; }
        public int? CreatedByUserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Comment { get; set; }
        public string Answer { get; set; }
        public DateTime? AnsweredDate { get; set; }
        public int? AnsweredByUserId { get; set; }
    }
}
