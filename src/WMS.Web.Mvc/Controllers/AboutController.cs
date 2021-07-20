using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using WMS.Controllers;

namespace WMS.Web.Controllers
{
    [AbpMvcAuthorize]
    public class AboutController : WMSControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
