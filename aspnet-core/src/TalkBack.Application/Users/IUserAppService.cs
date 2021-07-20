using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TalkBack.Roles.Dto;
using TalkBack.Users.Dto;

namespace TalkBack.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();

        Task ChangeLanguage(ChangeUserLanguageDto input);
    }
}
