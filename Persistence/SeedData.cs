using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class SeedData
    {
        public static async Task SeedActivities(DataContext context, UserManager<AppUser> userManager){
            
            if(!userManager.Users.Any()){
                var users = new List<AppUser>{
                  new AppUser{
                    DisplayName = "Edwin",
                    UserName = "eduuh",
                    Email = "eduuh@test.com"
                    },
                  new AppUser{
                    DisplayName = "jane",
                    UserName = "jane",
                    Email = "jane@test.com"
                    },
                  new AppUser{
                    DisplayName = "kim",
                    UserName = "kim",
                    Email = "kim@test.com"
                    },
                };
                
                foreach (var user in users)
                {
                    // this create the users and adds them to the store
                  await  userManager.CreateAsync(user, "Pa$$w0rd");

                }
            }

            if (!context.Activities.Any())
            {
                var activities = new List<Activity>{
                  new Activity {
                     Title = "Future activity 1" ,
                     Date = DateTime.Now.AddMonths(5),
                     Description = "Activity Number 1"
                  },
                  new Activity {
                     Title = "Future activity 1" ,
                     Date = DateTime.Now.AddMonths(5),
                     Description = "Activity Number 1",
                  }
            };
             context.Activities.AddRange(activities);
               await context.SaveChangesAsync();
            }
        }
        
    }
}