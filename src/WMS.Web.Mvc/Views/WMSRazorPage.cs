using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace WMS.Web.Views
{
    public abstract class WMSRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected WMSRazorPage()
        {
            LocalizationSourceName = WMSConsts.LocalizationSourceName;
        }
    }
}
