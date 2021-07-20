using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkBack
{
    public class Webinar : FullAuditedEntity<int>
    {
        [ForeignKey("ProjectDetails")]
        public int ProjectId { get; set; }
        public Project ProjectDetails { get; set; }

        public string Name { get; set; }

        public string Headline { get; set; }
        public string SubHeadline { get; set; }
        public string VideoURL { get; set; }
        
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public DateTime? RegistrationEndDate { get; set; }
        
        public float Cost { get;set;}
        public float DiscountedCost { get; set; }

        public string JoiningDetails { get; set; }
    }
}
