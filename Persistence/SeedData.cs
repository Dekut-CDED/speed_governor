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
                    Email = "eduuh@test.com",
                    SpeedGovernors = new List<SpeedGovernor> {
                     new SpeedGovernor {
                       Imei = "232324345454534",
                       Phone ="0753480483",
                       PlateNumber = "234232332",
                       Speeds = new List<Location> {
                          new Location {
                             Time = DateTime.Now.ToString(),
                             Latitude = -0.395154,
                             Long = 36.965040,
                             Speed = "57",
                             GpsCourse= "-3",
                             SpeedSignalStatus = "34",
                             EngineON = "34",
            },
                          new Location {
                             Time = DateTime.Now.ToString(),
                             Latitude = -0.395154,
                             Long = 36.965040,
                             Speed = "64",
                             GpsCourse= "-3",
                             SpeedSignalStatus = "34",
                             EngineON = "34",
            },
                          new Location {
                             Time = DateTime.Now.ToString(),
                             Latitude = -0.395154,
                             Long = 36.975040,
                             Speed = "79",
                             GpsCourse= "-3",
                             SpeedSignalStatus = "34",
                             EngineON = "34",
            },
                       }
                     } ,
                     new SpeedGovernor {
                       Imei = "232324345454534",
                       Phone ="0753480483",
                       PlateNumber = "234232332",
                       Speeds = new List<Location> {
                          new Location {
                             Time = DateTime.Now.ToString(),
                             Latitude = 0.395154,
                             Long = 36.975040,
                             Speed = "03",
                             GpsCourse= "-3",
                             SpeedSignalStatus = "34",
                             EngineON = "34",
            },
                          new Location {
                             Time = DateTime.Now.ToString(),
                             Latitude = 0.395154,
                             Long = 36.975040,
                             Speed = "11",
                             GpsCourse= "-3",
                             SpeedSignalStatus = "34",
                             EngineON = "34",
            },
                          new Location {
                             Time = DateTime.Now.ToString(),
                             Latitude = 0.395154,
                             Long = 36.975040,
                             Speed = "80",
                             GpsCourse= "-3",
                             SpeedSignalStatus = "34",
                             EngineON = "34",
            },
                       }
                     }   
                    }

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

                var commands = new List<SGCommand>
                {
                  new SGCommand {
                     Description = "Stop car",
                     Name = "3434",
                  },
                  new SGCommand {
                     Description = "View fuel",
                     Name = "3434",
                  }
                };

                foreach (var user in users)
                {
                    // this create the users and adds them to the store
                  await  userManager.CreateAsync(user, "Pa$$w0rd");

                }

                var activities = new List<UserActivity>
                {
                   new UserActivity {
                      AppUser = users[0],
                      Command = commands[0],
                   },
                   new UserActivity {
                      AppUser = users[1],
                      Command = commands[0],
                   },
                   new UserActivity {
                      AppUser = users[1],
                      Command = commands[1],
                   }
                };

                context.Commands.AddRange(commands);
                context.UserActivities.AddRange(activities);
                await context.SaveChangesAsync();
            }

        }
        
    }
}
