using System;
namespace Domain
{
    public class UserActivity
    {
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public int ActivityId { get; set; }
        public virtual SGCommand Command { get; set; }
        public DateTime DateTriggered { get; set; }
        public bool IsAdmin { get; set; }
    }
}