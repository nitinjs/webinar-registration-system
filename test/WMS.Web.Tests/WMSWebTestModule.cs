using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using WMS.EntityFrameworkCore;
using WMS.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace WMS.Web.Tests
{
    [DependsOn(
        typeof(WMSWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class WMSWebTestModule : AbpModule
    {
        public WMSWebTestModule(WMSEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(WMSWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(WMSWebMvcModule).Assembly);
        }
    }
}