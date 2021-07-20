using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using WMS.Configuration.Dto;

namespace WMS.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : WMSAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
