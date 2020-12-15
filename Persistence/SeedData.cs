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

        }
        
    }
}