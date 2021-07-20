using System.Collections.Generic;
using WMS.Roles.Dto;

namespace WMS.Web.Models.Roles
{
    public class RoleListViewModel
    {
        public IReadOnlyList<PermissionDto> Permissions { get; set; }
    }
}
