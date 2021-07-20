using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace TalkBack.EntityFrameworkCore
{
    public static class TalkBackDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<TalkBackDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<TalkBackDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
