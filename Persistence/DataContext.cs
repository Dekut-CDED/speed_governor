using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
        {

            public DataContext(DbContextOptions option) : base(option)
            {

            }

            public DbSet<Location> Locations { get; set; }
            public DbSet<SGCommand> Commands { get; set; }
            public DbSet<UserActivity> UserActivities { get; set; }
            public DbSet<SpeedGovernor> SpeedGovernors { get; set; }
            
            protected override void OnModelCreating(ModelBuilder builder){
              base.OnModelCreating(builder);

            // one to many relationships
            builder.Entity<SpeedGovernor>().HasOne(s => s.Owner).WithMany(s => s.SpeedGovernors);

            builder.Entity<Location>().HasOne(s => s.SpeedGovernor).WithMany(l => l.Speeds);


            // many to many relationship
            builder.Entity<UserActivity>(x => x.HasKey(ua => new { ua.AppUserId, ua.ActivityId }));

            builder.Entity<UserActivity>().HasOne(u => u.AppUser).WithMany(a => a.UserActivities).HasForeignKey(u => u.AppUserId);

            builder.Entity<UserActivity>().HasOne(u => u.Command).WithMany(a => a.UserActivities).HasForeignKey(u => u.ActivityId);
        }
        }
    }
