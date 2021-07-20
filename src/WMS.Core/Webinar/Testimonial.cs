using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS
{
    public class Testimonial:Entity<int>
    {
        public string PhotoPath { get; set; }
        public string Name { get; set; }
        public int NumberOfReviews { get; set; }
        public int NumberOfStars { get; set; }
        public string Positive { get; set; }
        public string Negative { get; set; }
        public string Review { get; set; }
        public int NumberOfLikes { get; set; }

        [ForeignKey("WebinarDetails")]
        public int? WebinarId { get; set; }//the one which doesn't have webinar id is a template
        public Webinar WebinarDetails { get; set; }
    }
}
