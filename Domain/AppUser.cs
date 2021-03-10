using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser: IdentityUser
    {
       public string DisplayName { get; set; }
       public string FirstName {get; set; }
       public string LastName {get; set; }
       public virtual ICollection<SpeedGovernor> SpeedGovernors{ get; set; }
       public virtual ICollection<UserActivity> UserActivities { get; set; }

        public override string ToString()
        {
            foreach (var item in SpeedGovernors)
            {
                foreach (var item2 in item.Locations) {
                    Console.WriteLine(item2.Latitude);
                }

            }
            return base.ToString();
        }
    }
}
