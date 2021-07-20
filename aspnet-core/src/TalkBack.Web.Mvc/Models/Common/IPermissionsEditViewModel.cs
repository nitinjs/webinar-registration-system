using System.Collections.Generic;
using TalkBack.Roles.Dto;

namespace TalkBack.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}