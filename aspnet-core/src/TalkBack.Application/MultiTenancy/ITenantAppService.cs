using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TalkBack.MultiTenancy.Dto;

namespace TalkBack.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

