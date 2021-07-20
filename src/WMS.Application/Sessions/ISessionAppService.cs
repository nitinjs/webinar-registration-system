using System.Threading.Tasks;
using Abp.Application.Services;
using WMS.Sessions.Dto;

namespace WMS.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
