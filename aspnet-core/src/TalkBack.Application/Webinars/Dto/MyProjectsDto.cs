using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkBack.Webinars.Dto
{
    public class MyProjectsDto
    {
        public MyProjectsDto()
        {
            TotalCount = 0;
            Items = new List<ProjectDto>();
        }

        public int TotalCount { get; set; }
        public List<ProjectDto> Items { get; set; }
    }
}
