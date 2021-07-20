using System.Threading.Tasks;
using TalkBack.Configuration.Dto;

namespace TalkBack.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
