using System.Threading.Tasks;
using Abp.Application.Services;
using TalkBack.Authorization.Accounts.Dto;

namespace TalkBack.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
