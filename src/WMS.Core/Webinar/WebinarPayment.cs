using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS
{
    public class WebinarPayment : FullAuditedEntity<int>
    {
        public int WebinarId { get; set; }
        [ForeignKey("WebinarId")]
        public Webinar WebinarDetails { get; set; }

        public string ReferenceId { get; set; }
        public bool IsFeePaid { get; set; }
        public DateTime? FeePaidOn { get; set; }

        public long UserId { get; set; }//Id of the user who pays for the webinar
    }
}
