using Abp.Authorization;
using WMS.Authorization.Roles;
using WMS.Authorization.Users;

namespace WMS.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
