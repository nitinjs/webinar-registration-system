using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using TalkBack.Configuration;
using Abp.Dependency;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Hangfire;

namespace TalkBack.Web.Startup
{
    [DependsOn(typeof(TalkBackWebCoreModule))]
    [DependsOn(typeof(AbpHangfireModule))]
    public class TalkBackWebMvcModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public TalkBackWebMvcModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        private static IOptions<PayPalAuthOptions> GetAppSettingOptions()
        {
            var services = new ServiceCollection();
            services.AddOptions();
            var configurationRoot = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());
            services.Configure<PayPalAuthOptions>(configurationRoot.GetSection(nameof(PayPalAuthOptions)).Bind);
            var serviceProvider = services.BuildServiceProvider();
            var appSettingOptions = serviceProvider.GetRequiredService<IOptions<PayPalAuthOptions>>();
            return appSettingOptions;
        }

        public override void PreInitialize()
        {
            Configuration.Navigation.Providers.Add<TalkBackNavigationProvider>();
            Configuration.BackgroundJobs.UseHangfire(conf=> {                
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TalkBackWebMvcModule).GetAssembly());
            //IocManager.IocContainer.Register(Component.For<IOptions<PayPalAuthOptions>>()
            //    .UsingFactoryMethod(x => GetAppSettingOptions()));
            //IocManager.Register(typeof(PaypalServices), DependencyLifeStyle.Transient);
            //Configuration.IocManager.Register<IPaypalServices, PaypalServices>(DependencyLifeStyle.Singleton);
            //IocManager.IocContainer.Register(Classes.FromThisAssembly().BasedOn<IPaypalServices>().LifestylePerThread().WithServiceSelf());
        }
    }
}
