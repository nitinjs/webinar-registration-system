using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using TalkBack;
using TalkBack.Authorization.Users;
using TalkBack.Controllers;
using TalkBack.Web.Models;
using TalkBack.Webinars;
using TalkBack.Webinars.Dto;

namespace TalkBack.Web.Controllers
{
    public class WebinarsController : TalkBackControllerBase
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
        private readonly IOptions<MyConfig> config;
        public IRepository<Benefit> benefitsRepo { get; set; }
        public IRepository<SpeakerPofile> speakerProfileRepo { get; set; }
        public IRepository<Testimonial> testimonialRepo { get; set; }

        public WebinarsController(IUnitOfWorkManager unitOfWorkManager,IOptions<MyConfig> config)
        {
            _unitOfWorkManager = unitOfWorkManager;
            this.config = config;
        }

        public IActionResult Index(string id)
        {
            var projectid = Convert.ToInt32(id);
            ViewBag.Domain = config.Value.Domain;
            ViewBag.Id = projectid;
            ViewBag.Webinars = webinarService.GetWebinarsByProjectId(new IdDto<int>(Convert.ToInt32(id)));
            return View();
        }

        public IActionResult V(string id)
        {
            return RedirectToAction("Preview", new { id = id});
        }

        public bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
                if (number % i == 0)
                    return false;

            return true;
        }

        public IActionResult Preview(string id)
        {
            int wid = Convert.ToInt32(id);
            var d = designRepo.GetAllIncluding(x=>x.WebinarDetails).Where(x => x.WebinarId == wid).FirstOrDefault();
            if (d != null)
            {
                string Html = d.Html.Replace("<%HEADLINE%>", d.WebinarDetails.Headline);
                Html = Html.Replace("&lt;%HEADLINE%&gt;", d.WebinarDetails.Headline);

                Html = Html.Replace("<%SUBHEADLINE%>", d.WebinarDetails.SubHeadline);
                Html = Html.Replace("&lt;%SUBHEADLINE%&gt;", d.WebinarDetails.SubHeadline);

                string link = config.Value.Domain + "/Payment/Register/" + d.WebinarDetails.Id;
                string url = "<a class=\"btn btn-success\" href=\"" + link + "\">Register</a>";
                Html = Html.Replace("<%REGISTER%>", url);
                Html = Html.Replace("&lt;%REGISTER%&gt;", url);

                Html = Html.Replace("<%REGISTERLINK%>", link);
                Html = Html.Replace("&lt;%REGISTERLINK%&gt;", link);

                string videourl = d.WebinarDetails.VideoURL;
                if (!string.IsNullOrWhiteSpace(videourl) && videourl.Contains("/"))
                {
                    string[] splittedV = videourl.Split("/", StringSplitOptions.RemoveEmptyEntries);
                    videourl = "https://www.youtube.com/embed/" + splittedV.Last().Replace("watch?v=", "");
                    Html = Html.Replace("<%VIDEOURL%>", videourl);
                    Html = Html.Replace("&lt;%VIDEOURL%&gt;", videourl);
                }

                string benefitsHTML = "";
                var benefits = benefitsRepo.GetAll().Where(x => x.WebinarId == wid).ToList();
                int count = 0;
                foreach (var b in benefits)
                {
                    var html = "";
                    count++;
                    if (count%2 !=0 || count==1)
                    {
                        html += @"<div class=""col-sm-1""></div>";
                    }
                    html += @"<div class=""col-sm-5"">
                                    <div class=""col_text"">
                                        <img src="""+config.Value.Domain + @"/templates/img/check.png"">
                                        <p>
                                            <strong>"+ b.Name +@"</strong>
                                            <br>
                                            "+ b.Description.Replace(Environment.NewLine, "<br/>") + @"
                                        </p>
                                    </div>
                                </div>";
                    if (count%2==0)
                    {
                        html += @"<div class=""col-sm-1""></div>";
                    }
                    benefitsHTML += html;
                }
                Html = Html.Replace("<%BENEFITS%>", benefitsHTML);
                Html = Html.Replace("&lt;%BENEFITS%&gt;", benefitsHTML);

                string speakerProfileHTML = "";
                var speakerProfiles = speakerProfileRepo.GetAll().Where(x => x.WebinarId == wid).ToList();

                int count2=0;
                foreach (var sp in speakerProfiles)
                {
                    count2++;
                    if (count2 % 2 != 0 || count2 == 1)
                    {
                        speakerProfileHTML += @"<div class=""container mt-3 mb-3"">
                                              <div class=""row"">
                                                <DIV CLASS=""col-md-1""></DIV>
                                                <div class=""col-md-2"">
                                                    <div class=""profile-img"">
                                                        <img src=""" + sp.PhotoPath + @""" style=""width:100%""/>
                                                    </div>
                                                </div>
                                                <div class=""col-md-8"">
                                                    <div class=""profile-head"">
                                                                <h3 style=""color:#A53528"">
                                                                    " + sp.Name + @"
                                                                </h3>
                                                                <h6>
                                                                    <a style=""color:gray"" href=""" + sp.Website + @""" target=""_blank"">" + sp.Website + @"</a>
                                                                </h6>
                                                                <p>
                                                                    " + sp.Description.Replace(Environment.NewLine, "<br/>") + @"
                                                                </p>
                                                    </div>
                                                </div>
                                                <DIV CLASS=""col-md-1""></DIV>
                                            </div>
                                        </div>";
                    }
                    else {
                        speakerProfileHTML += @"<div class=""container mt-3 mb-3"">
                                              <div class=""row"">
                                                <DIV CLASS=""col-md-1""></DIV>
                                                <div class=""col-md-8"">
                                                    <div class=""profile-head"">
                                                                <h3 style=""color:#A53528"">
                                                                    " + sp.Name + @"
                                                                </h3>
                                                                <h6>
                                                                    <a style=""color:gray"" href=""" + sp.Website + @""" target=""_blank"">" + sp.Website + @"</a>
                                                                </h6>
                                                                <p>
                                                                    " + sp.Description.Replace(Environment.NewLine, "<br/>") + @"
                                                                </p>
                                                    </div>
                                                </div>
                                                <div class=""col-md-2"">
                                                    <div class=""profile-img"">
                                                        <img src=""" + sp.PhotoPath + @""" style=""width:100%""/>
                                                    </div>
                                                </div>
                                                <DIV CLASS=""col-md-1""></DIV>
                                            </div>
                                        </div>";
                    }
                } 
                Html = Html.Replace("<%SPEAKERPROFILES%>", speakerProfileHTML);
                Html = Html.Replace("&lt;%SPEAKERPROFILES%&gt;", speakerProfileHTML);

                string testimonialHTML = "";
                var testimonials = testimonialRepo.GetAll().Where(x => x.WebinarId == wid).ToList();

                int count3 = 0;
                foreach (var t in testimonials)
                {
                    string starsHTML = "<table><tr>";
                    for (int i = 1; i <= t.NumberOfStars; i++)
                    {
                        starsHTML += "<td><img src='/templates/img/testimonials/star.png' /></td>";
                    }
                    starsHTML += "</tr></table>";
                    count3++;
                    testimonialHTML += @"<div class=""row"">
                                            <div class=""col-md-2""></div>
	                                        <div class=""col-md-8"">
                                                       <section class=""post-heading"">
                                                            <div class=""row"">
                                                                <div class=""col-md-12"">
                                                                    <div class=""media"">
                                                                      <div class=""media-left"">
                                                                          <img class=""media-object photo-profile"" src="""+ t.PhotoPath + @""" width='80' height='80' style='-webkit-border-radius: 80px;-moz-border-radius: 80px;border-radius: 80px;' alt='...'>
                                                                      </div>
                                                                      <div class='media-body ml-3'>
                                                                        <h5 class='media-heading' style='margin-bottom:0px'>" + t.Name +@"</h5>
                                                                        "+ starsHTML + @""+ t.NumberOfReviews+@" reviews
                                                                      </div>
                                                                    </div>
                                                                </div>
                                                            </div>             
                                                       </section>
                                                       <section class='post-body'>
                                                           <p>
                                                            "+ t.Review.Replace(Environment.NewLine,"<br/>") + @"
                                                            </p>
                                                       </section>
                                                       <section class='post-footer'>
                                                           <hr>
                                                           <div class='post-footer-option container'>
                                                                 <ul class='list-unstyled'>
                                                                    <li><table><tr><td><img style=""width:30px;height:30px"" src=""/templates/img/testimonials/thumb.png""/></td><td class='pl-1'>" + t.NumberOfLikes +@"</td></tr></table></li>
                                                                </ul>
                                                           </div>
                                                       </section>
	                                        </div>
                                            <div class='col-md-2'></div>
                                        </div>";
                }
                Html = Html.Replace("<%TESTIMONIALS%>", testimonialHTML);
                Html = Html.Replace("&lt;%TESTIMONIALS%&gt;", testimonialHTML);

                ViewBag.Html = Html;
            }
            else {
                ViewBag.Html = "design not found";
            }
            return View();
        }

        public IActionResult Add(string id)
        {
            var projectid = Convert.ToInt32(id);
            WebinarEditDto dto = new WebinarEditDto();
            dto.ProjectId = projectid;
            dto.Templates = webinarAppService.GetAllTemplates().Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            dto.Templates.Insert(0, new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = "--Select Template--", Value = "0" });
            return View(dto);
        }

        [HttpPost]
        public IActionResult Add(WebinarEditDto model)
        {
            try
            {
                var id = webinarAppService.AddWebinar(model);
                return RedirectToAction("Index","Webinars", new { id = model.ProjectId });
            }
            catch (Exception ex)
            {
                model.Templates = webinarAppService.GetAllTemplates().Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
                model.Templates.Insert(0, new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = "--Select Template--", Value = "0" });
                return View(model);
            }
        }

        public IActionResult Edit(string id)
        {
            var model = webinarAppService.GetWebinar(new IdDto<int>(Convert.ToInt32(id)));
            model.DesignHtml = webinarAppService.GetTemplateForWebinar(new IdDto<int>(model.Id))?.Html;
            WebinarEditDto dto = new WebinarEditDto(model);
            dto.Templates = webinarAppService.GetAllTemplates().Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            dto.Templates.Insert(0, new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = "--Select Template--", Value = "0" });
            return View(dto);
        }

        [HttpPost]
        public IActionResult Edit(WebinarEditDto model)
        {
            try
            {
                var id = webinarAppService.EditWebinar(model);
                return RedirectToAction("Index", "Webinars", new { id = model.ProjectId });
            }
            catch (Exception ex)
            {
                model.Templates = webinarAppService.GetAllTemplates().Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();

                model.Templates.Insert(0, new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = "--Select Template--", Value = "0" });
                return View(model);
            }
        }
    }
}
