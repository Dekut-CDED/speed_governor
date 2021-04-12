using System;

namespace Application.User
{
    public class User
    {

        public string Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }


    }
}
