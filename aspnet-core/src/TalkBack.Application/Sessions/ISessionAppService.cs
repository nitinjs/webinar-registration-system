using System.Threading.Tasks;
using Abp.Application.Services;
using TalkBack.Sessions.Dto;

namespace TalkBack.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
