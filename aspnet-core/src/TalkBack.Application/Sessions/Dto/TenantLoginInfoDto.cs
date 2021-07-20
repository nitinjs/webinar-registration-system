using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using TalkBack.MultiTenancy;

namespace TalkBack.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}
