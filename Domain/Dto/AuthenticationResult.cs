using System.Collections;

namespace Domain
{
    public class AuthenticationResult
    {
        public IEnumerable Errors { get; set; }
        public bool Success { get; set; }
        public string Info { get; set; }
    }
}