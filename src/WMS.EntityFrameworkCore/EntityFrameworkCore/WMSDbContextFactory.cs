using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using WMS.Configuration;
using WMS.Web;

namespace WMS.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class WMSDbContextFactory : IDesignTimeDbContextFactory<WMSDbContext>
    {
        public WMSDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<WMSDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            WMSDbContextConfigurer.Configure(builder, configuration.GetConnectionString(WMSConsts.ConnectionStringName));

            return new WMSDbContext(builder.Options);
        }
    }
}
