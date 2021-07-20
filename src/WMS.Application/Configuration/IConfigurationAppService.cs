using System.Threading.Tasks;
using WMS.Configuration.Dto;

namespace WMS.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
