using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using WMS.Controllers;
using Abp.Domain.Uow;
using WMS.Authorization.Users;
using Abp.Domain.Repositories;
using WMS.Webinars;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Net.Mail;
using System;
using System.Linq;
using WMS.Webinars.Dto;
using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WMS.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : WMSControllerBase
    {
        public SmtpClient smtpClient { get; set; }
        public IHostingEnvironment _hostingEnvironment { get; set; }
        public IConfiguration iConfig { get; set; }
        public IWebinarAppService webinarAppService { get; set; }
        public IRepository<User, long> UserRepo { get; set; }
        public IRepository<WebinarPayment> paymentRepo { get; set; }
        //public IUserAppService userAppService { get; set; }
        public UserRegistrationManager _userRegistrationManager { get; set; }
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public IRepository<WebinarDesign> designRepo { get; set; }
        public IRepository<Webinar> webinarRepo { get; set; }
        public HomeController(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }
        public ActionResult Index()
        {
            if (webinarAppService.IsAdmin(new EntityDto<long>() { Id = AbpSession.UserId.Value }))
            {
                ViewBag.Projects = ProjectsRepo.GetAll().Count();
                ViewBag.Webinars = webinarRepo.GetAll().Count();
                ViewBag.Sales = paymentRepo.GetAll().Count();
                ViewBag.Users = UserRepo.GetAll().Count();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "MyWebinars");
            }
        }

        public IActionResult ImportTemplates()
        {
            designRepo.Delete(x => x.WebinarId==null);

            string[] files = System.IO.Directory.GetFiles(_hostingEnvironment.WebRootPath + "\\templates\\", "*.html");
            foreach (string file in files)
            {
                WebinarDesign d = new WebinarDesign();
                d.CreationTime = DateTime.Now;
                d.CreatorUserId = AbpSession.UserId;
                d.Name = System.IO.Path.GetFileNameWithoutExtension(new System.IO.FileInfo(file).Name);
                d.Html = System.IO.File.ReadAllText(file);
                designRepo.Insert(d);
            }
            return Content("success");
        }

        [HttpPost]
        public async Task<ActionResult> Upload(List<IFormFile> SpeakerProfilePhoto)
        {
            string uploadPath = _hostingEnvironment.WebRootPath + "\\files\\";
            if (!System.IO.Directory.Exists(uploadPath)) {
                System.IO.Directory.CreateDirectory(uploadPath);
            }

            if (SpeakerProfilePhoto.Count != 0)
            {
                for (int i = 0; i < SpeakerProfilePhoto.Count; i++)
                {
                    var file = SpeakerProfilePhoto[i];
                    var fileName = System.Guid.NewGuid().ToString()+" "+ System.IO.Path.GetFileName(file.FileName);
                    var filePath = System.IO.Path.Combine(uploadPath, fileName);

                    using (var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return Json(new { path = "/files/" + fileName });
                }
            }
            return null;
        }
    }
}
