using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkBack.Webinars.Dto
{
    public class PaymentListDto
    {
        public PaymentListDto()
        {
            TotalCount = 0;
            Items = new List<PaymentDto>();
        }

        public int TotalCount { get; set; }
        public List<PaymentDto> Items { get; set; }
    }

    public class PaymentDto
    { 
        public int Id { get; set; }
        public string ReferenceId { get; set; }
        public bool IsFeePaid { get; set; }
        public DateTime? FeePaidOn { get; set; }

        public string PayerName { get; set; }

        /// <summary>
        /// Id of the user who pays for the webinar
        /// </summary>
        public long UserId { get; set; }

        public int WebinarId { get; set; }
        public string Name { get; set; }
        public string Headline { get; set; }
        public float Cost { get; set; }
        public float DiscountedCost { get; set; }
    }
}
