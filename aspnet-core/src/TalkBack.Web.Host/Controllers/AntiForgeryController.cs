using Microsoft.AspNetCore.Antiforgery;
using TalkBack.Controllers;

namespace TalkBack.Web.Host.Controllers
{
    public class AntiForgeryController : TalkBackControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
