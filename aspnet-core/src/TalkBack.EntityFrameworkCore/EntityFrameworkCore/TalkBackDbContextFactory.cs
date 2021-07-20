using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TalkBack.Configuration;
using TalkBack.Web;

namespace TalkBack.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class TalkBackDbContextFactory : IDesignTimeDbContextFactory<TalkBackDbContext>
    {
        public TalkBackDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<TalkBackDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            TalkBackDbContextConfigurer.Configure(builder, configuration.GetConnectionString(TalkBackConsts.ConnectionStringName));

            return new TalkBackDbContext(builder.Options);
        }
    }
}
