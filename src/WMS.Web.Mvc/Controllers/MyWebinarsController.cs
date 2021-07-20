using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Controllers;

namespace WMS.Web.Controllers
{
    public class MyWebinarsController : WMSControllerBase
    {
        public IActionResult Index()
        {
            ViewBag.Id = AbpSession.UserId;
            return View();
        }
    }
}
