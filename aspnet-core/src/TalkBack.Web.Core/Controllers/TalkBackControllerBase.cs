using Abp.AspNetCore.Mvc.Controllers;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Mail;
using TalkBack.Webinars;

namespace TalkBack.Controllers
{
    public abstract class TalkBackControllerBase: AbpController
    {
        public SmtpClient smtpClient { get; set; }
        public IHostingEnvironment _hostingEnvironment { get; set; }
        public IConfiguration iConfig { get; set; }
        public IRepository<Project> ProjectsRepo { get; set; }
        public IRepository<Webinar> WebinarRepo { get; set; }
        public IRepository<WebinarDesign> DesignRepo { get; set; }
        public IWebinarAppService webinarService { get; set; }
        protected TalkBackControllerBase()
        {
            LocalizationSourceName = TalkBackConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        [NonAction]
        public void SendMail(string to, string subject, string body)
        {
            var msg = new MailMessage()
            {
                From = new MailAddress("siform-do-not-reply@septicrx.com", "Septic Rx"),
                Subject = subject,
                Body = body
            };
            string[] mails = to.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var m in mails)
                msg.To.Add(m);
            msg.IsBodyHtml = true;
            //msg.Attachments.Add(new Attachment());

            this.smtpClient.Send(msg);
        }
    }
}
