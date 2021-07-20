using System.Collections.Generic;
using TalkBack.Roles.Dto;
using TalkBack.Users.Dto;

namespace TalkBack.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<UserDto> Users { get; set; }

        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}
