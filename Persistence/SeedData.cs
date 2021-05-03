using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{

    public class SeedData
    {
        private static readonly Random _random = new Random();
        string[] roles = new[] { "admin", "retailer", "user" };
        private static readonly string v = $"{Guid.NewGuid()}";
        public static Faker<AppUser> Fakeusers { get; } = new Faker<AppUser>()
                                 .RuleFor(p => p.FirstName, f => f.Name.FindName())
                                 .RuleFor(p => p.LastName, f => f.Name.LastName())
                                 .RuleFor(p => p.UserName, f => f.Name.LastName())
                                 .RuleFor(p => p.Email, f => f.Internet.Email())
                                 .RuleFor(p => p.PhoneNumber, f => f.Phone.PhoneNumber())
                                 .RuleFor(p => p.SpeedGovernors, f => FakeSpeedGovernor.Generate(10));
        public static Faker<Location> FakeLocations { get; } = new Faker<Location>()
                                  .RuleFor(l => l.Time, f => DateTime.Now.ToString())
                                  .RuleFor(l => l.Latitude, f => f.Address.Latitude())
                                  .RuleFor(l => l.Long, f => f.Address.Longitude())
                                  .RuleFor(l => l.Speed, "54")
                                  .RuleFor(l => l.SpeedSignalStatus, "54")
                                  .RuleFor(l => l.EngineON, "54");
        public static Faker<SpeedGovernor> FakeSpeedGovernor { get; } = new Faker<SpeedGovernor>()
                                   .RuleFor(s => s.Imei, f => Guid.NewGuid().ToString())
                                   .RuleFor(s => s.Phone, f => f.Phone.PhoneNumber())
                                   .RuleFor(s => s.PlateNumber, f => f.Vehicle.Vin())
                                   .RuleFor(s => s.Fuellevel, f => f.Vehicle.Fuel())
                                   .RuleFor(s => s.Locations, f => FakeLocations.Generate(10))
                                   .RuleFor(s => s.Vibrations, f => f.Vehicle.Fuel());

        public static Faker<SGCommandActivity> FakeActivity { get; } = new Faker<SGCommandActivity>()
                                     .RuleFor(s => s.Description, f => f.Company.CatchPhrase())
                                     .RuleFor(s => s.Name, f => f.Company.CompanyName());

        public static async Task SeedActivities(DataContext context, UserManager<AppUser> userManager)
        {

            #region test
            // Use this to seed the speed governor for tests
            var speedgov = new SpeedGovernor
            {
                Phone = "07152094578",
                Imei = "451282484",
                PlateNumber = "KCU 808H",
                OwnerId = "83603a79-4ef0-48f8-9c57-b805aa009e1c"
            };

            context.SpeedGovernors.Add(speedgov);
            await context.SaveChangesAsync();
            #endregion

            var usersfake = Fakeusers.Generate(100);
            var fakeactivities = FakeActivity.Generate(100);
            var useractivities = new List<UserActivity>();
            //int i = 0;
            // foreach (var user in usersfake)
            // {
            //     useractivities.Add(new UserActivity() { ActivityId = fakeactivities[i].Id, Command = fakeactivities[i], AppUser = user, AppUserId = user.Id, IsAdmin = true, DateTriggered = DateTime.Now });
            //     i++;
            // }
            var admin = new AppUser
            {
                FirstName = "John",
                LastName = "Wahome",
                UserName = "johnwahome",
                Email = "john@test.com",
                SpeedGovernors = FakeSpeedGovernor.Generate(8)
            };
            usersfake.Add(admin);
            if (!userManager.Users.Any())
            {
                foreach (var user in usersfake)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }
                context.Commands.AddRange(fakeactivities);
                context.UserActivities.AddRange(useractivities);
                await context.SaveChangesAsync();
            }

        }

    }
}

