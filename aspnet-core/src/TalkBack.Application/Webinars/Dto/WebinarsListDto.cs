using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkBack.Webinars.Dto
{
    public class WebinarsListDto
    {
        public WebinarsListDto()
        {
            TotalCount = 0;
            Items = new List<WebinarDto>();
        }

        public int TotalCount { get; set; }
        public List<WebinarDto> Items { get; set; }
    }
}
