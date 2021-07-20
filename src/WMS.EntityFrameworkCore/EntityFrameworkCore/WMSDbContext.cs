using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using WMS.Authorization.Roles;
using WMS.Authorization.Users;
using WMS.MultiTenancy;


namespace WMS.EntityFrameworkCore
{
    public class WMSDbContext : AbpZeroDbContext<Tenant, Role, User, WMSDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Project> Projects { get; set; }
        public DbSet<Webinar> Webinars { get; set; }
        public DbSet<WebinarDesign> Designs { get; set; }
        public DbSet<WebinarPayment> Payments { get; set; }

        public DbSet<Benefit> Benefits { get; set; }
        public DbSet<SpeakerPofile> SpeakerProfiles { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }

        public WMSDbContext(DbContextOptions<WMSDbContext> options)
            : base(options)
        {
        }
    }
}
