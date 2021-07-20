using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace WMS.EntityFrameworkCore
{
    public static class WMSDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<WMSDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<WMSDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
