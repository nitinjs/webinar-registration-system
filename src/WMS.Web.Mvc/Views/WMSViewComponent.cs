using Abp.AspNetCore.Mvc.ViewComponents;

namespace WMS.Web.Views
{
    public abstract class WMSViewComponent : AbpViewComponent
    {
        protected WMSViewComponent()
        {
            LocalizationSourceName = WMSConsts.LocalizationSourceName;
        }
    }
}
