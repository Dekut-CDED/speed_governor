using System;
using System.Collections.Generic;

namespace Domain
{
    public class SGCommand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserActivity> UserActivities { get; set; }
    }
}