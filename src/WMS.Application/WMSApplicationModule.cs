using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using WMS.Authorization;

namespace WMS
{
    [DependsOn(
        typeof(WMSCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class WMSApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<WMSAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(WMSApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
