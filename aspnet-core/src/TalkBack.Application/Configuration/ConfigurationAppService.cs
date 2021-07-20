using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using TalkBack.Configuration.Dto;

namespace TalkBack.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : TalkBackAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
