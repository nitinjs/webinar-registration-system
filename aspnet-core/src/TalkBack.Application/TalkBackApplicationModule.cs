using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using TalkBack.Authorization;

namespace TalkBack
{
    [DependsOn(
        typeof(TalkBackCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class TalkBackApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<TalkBackAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(TalkBackApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );
        }
    }
}
