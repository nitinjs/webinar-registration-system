using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using TalkBack.Configuration;

namespace TalkBack.Web.Host.Startup
{
    [DependsOn(
       typeof(TalkBackWebCoreModule))]
    public class TalkBackWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public TalkBackWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TalkBackWebHostModule).GetAssembly());
        }
    }
}
