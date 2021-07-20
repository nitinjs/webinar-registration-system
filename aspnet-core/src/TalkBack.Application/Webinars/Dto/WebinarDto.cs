using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkBack.Webinars.Dto
{
    public class WebinarDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }

        public string Headline { get; set; }
        public string SubHeadline { get; set; }
        public string VideoURL { get; set; }

        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public DateTime? RegistrationEndDate { get; set; }

        public float Cost { get; set; }
        public float DiscountedCost { get; set; }

        public string JoiningDetails { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreationTime { get; set; }

        public string DesignHtml { get; set; }
    }
}
