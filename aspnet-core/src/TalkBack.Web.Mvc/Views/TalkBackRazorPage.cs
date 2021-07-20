using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;

namespace TalkBack.Web.Views
{
    public abstract class TalkBackRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected TalkBackRazorPage()
        {
            LocalizationSourceName = TalkBackConsts.LocalizationSourceName;
        }
    }
}
