using Abp.AspNetCore.Mvc.ViewComponents;

namespace TalkBack.Web.Views
{
    public abstract class TalkBackViewComponent : AbpViewComponent
    {
        protected TalkBackViewComponent()
        {
            LocalizationSourceName = TalkBackConsts.LocalizationSourceName;
        }
    }
}
