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
    public class WebinarDesign : FullAuditedEntity<int>
    {
        [ForeignKey("WebinarDetails")]
        public int? WebinarId { get; set; }//the one which doesn't have webinar id is a template
        public Webinar WebinarDetails { get; set; }

        public string Name { get; set; }

        public string Html { get; set; }
        public string Css { get; set; }
        public string JavaScript { get; set; }
        //public bool IsTemplate { get; set; }
    }
}
