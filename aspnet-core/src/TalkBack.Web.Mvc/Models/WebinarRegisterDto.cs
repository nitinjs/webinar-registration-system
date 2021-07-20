using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TalkBack.Web.Models
{
    public class WebinarRegisterDto
    {
        public int WebinarId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public float WebinarFee { get; set; }
    }
}
