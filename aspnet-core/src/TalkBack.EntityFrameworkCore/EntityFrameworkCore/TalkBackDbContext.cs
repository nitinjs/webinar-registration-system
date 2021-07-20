using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using TalkBack.Authorization.Roles;
using TalkBack.Authorization.Users;
using TalkBack.MultiTenancy;

namespace TalkBack.EntityFrameworkCore
{
    public class TalkBackDbContext : AbpZeroDbContext<Tenant, Role, User, TalkBackDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Project> Projects { get; set; }
        public DbSet<Webinar> Webinars { get; set; }
        public DbSet<WebinarDesign> Designs { get; set; }
        public DbSet<WebinarPayment> Payments { get; set; }

        public DbSet<Benefit> Benefits { get; set; }
        public DbSet<SpeakerPofile> SpeakerProfiles { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }

        public TalkBackDbContext(DbContextOptions<TalkBackDbContext> options)
            : base(options)
        {
        }
    }
}
