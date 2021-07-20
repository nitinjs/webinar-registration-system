using System.Collections.Generic;
using WMS.Roles.Dto;

namespace WMS.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}