using Abp.Authorization;
using TalkBack.Authorization.Roles;
using TalkBack.Authorization.Users;

namespace TalkBack.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
