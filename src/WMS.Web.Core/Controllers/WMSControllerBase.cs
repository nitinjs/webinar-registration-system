using Abp.AspNetCore.Mvc.Controllers;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mail;
using WMS.Webinars;

namespace WMS.Controllers
{
    public abstract class WMSControllerBase : AbpController
    {
        public SmtpClient smtpClient { get; set; }
        public IRepository<Project> ProjectsRepo { get; set; }
        public IRepository<Webinar> WebinarRepo { get; set; }
        public IRepository<WebinarDesign> DesignRepo { get; set; }
        public IWebinarAppService webinarService {get;set;}

        protected WMSControllerBase()
        {
            LocalizationSourceName = WMSConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
