using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS
{
    public class SpeakerPofile:Entity<int>
    {
        public string Name { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string PhotoPath { get; set; }

        [ForeignKey("WebinarDetails")]
        public int? WebinarId { get; set; }//the one which doesn't have webinar id is a template
        public Webinar WebinarDetails { get; set; }
    }
}
