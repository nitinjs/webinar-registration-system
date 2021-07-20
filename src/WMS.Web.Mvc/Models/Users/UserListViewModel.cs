using System.Collections.Generic;
using WMS.Roles.Dto;

namespace WMS.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}
