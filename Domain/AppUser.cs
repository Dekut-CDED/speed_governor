using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser: IdentityUser
    {
       public string DisplayName { get; set; }
       public string Imei { get; set; }

        public virtual ICollection<SpeedGovernor> SpeedGovernors{ get; set; }
        public virtual ICollection<UserActivity> UserActivities { get; set; }
    }
}
