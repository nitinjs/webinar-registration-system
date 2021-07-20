using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using WMS.Authorization;

namespace WMS.Web.Startup
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class WMSNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.Home,
                        L("Dashboard"),
                        url: "",
                        icon: "fas fa-home",
                        requiresAuthentication: true,
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Users),
                        order: 1
                    )
                //).AddItem( // Menu items below is just for demonstration!
                //    new MenuItemDefinition(
                //        PageNames.Projects,
                //        L("Projects"),
                //        url: "Projects",
                //        icon: "fas fa-project-diagram",
                //        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Users),
                //        order: 2
                //    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.MyWebinars,
                        L("My Webinars"),
                        url: "MyWebinars",
                        icon: "fas fa-photo-video",
                        order: 3
                    )
                ).AddItem( 
                    new MenuItemDefinition(
                        PageNames.Payments,
                        L("Payments"),
                        url: "Payment",
                        icon: "fas fa-money-check-alt",
                        order: 4
                    )
                //).AddItem(
                //    new MenuItemDefinition(
                //        PageNames.Tenants,
                //        L("Tenants"),
                //        url: "Tenants",
                //        icon: "fas fa-building",
                //        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Tenants),
                //        order: 5
                //    )
                //).AddItem(
                //    new MenuItemDefinition(
                //        PageNames.Roles,
                //        L("Roles"),
                //        url: "Roles",
                //        icon: "fas fa-theater-masks",
                //        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Roles),
                //        order: 6
                //            )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Users,
                        L("Users"),
                        url: "Users",
                        icon: "fas fa-users",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Users),
                        order: 7
                    )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, WMSConsts.LocalizationSourceName);
        }
    }
}
