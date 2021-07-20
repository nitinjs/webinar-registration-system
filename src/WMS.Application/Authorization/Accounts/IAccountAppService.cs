using System.Threading.Tasks;
using Abp.Application.Services;
using WMS.Authorization.Accounts.Dto;

namespace WMS.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
