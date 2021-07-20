using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using TalkBack.Controllers;

namespace TalkBack.Web.Controllers
{
    [AbpMvcAuthorize]
    public class AboutController : TalkBackControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
