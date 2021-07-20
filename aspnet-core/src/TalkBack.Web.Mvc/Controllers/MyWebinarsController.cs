using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalkBack.Controllers;

namespace TalkBack.Web.Controllers
{
    public class MyWebinarsController : TalkBackControllerBase
    {
        public IActionResult Index()
        {
            ViewBag.Id = AbpSession.UserId;
            ViewBag.Webinars = webinarService.GetWebinarsByUserId(new Webinars.Dto.IdDto<long>(AbpSession.UserId.Value));
            return View();
        }
    }
}
