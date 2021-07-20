using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Webinars.Dto;

namespace WMS.Web.Models
{
    public class WebinarEditDto: WebinarDto
    {
        public WebinarEditDto() {
            Templates = new List<SelectListItem>();
        }
        public WebinarEditDto(WebinarDto dto)
        {
            Templates = new List<SelectListItem>();
            this.Cost = dto.Cost;
            this.CreatedBy = dto.CreatedBy;
            this.CreationTime = dto.CreationTime;
            this.DesignHtml = dto.DesignHtml;
            this.DiscountedCost = dto.DiscountedCost;
            this.EndDateTime = dto.EndDateTime;
            this.Headline = dto.Headline;
            this.Id = dto.Id;
            this.JoiningDetails = dto.JoiningDetails;
            this.Name = dto.Name;
            this.ProjectId = dto.ProjectId;
            this.RegistrationEndDate = dto.RegistrationEndDate;
            this.StartDateTime = dto.StartDateTime;
            this.SubHeadline = dto.SubHeadline;
            this.VideoURL = dto.VideoURL;
        }
        public List<SelectListItem> Templates { get; set; }
        public int SelectedTemplateId { get; set; }
    }
}
