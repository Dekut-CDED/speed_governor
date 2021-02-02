using System;

namespace Application.User
{
    public class User
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumer {get; set;}
        public int AccessFailedCount { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; } 
        
    }
}
