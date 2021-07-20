using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Webinars.Dto
{
    public class TemplateDto
    {
        public int Id { get; set; }
        public int? WebinarId { get; set; }//the one which doesn't have webinar id is a template

        public string Name { get; set; }

        public string Html { get; set; }
        public string Css { get; set; }
        public string JavaScript { get; set; }
    }
}
