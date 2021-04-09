using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<SpeedGovernor> SpeedGovernors { get; set; }
        public virtual ICollection<UserActivity> UserActivities { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
