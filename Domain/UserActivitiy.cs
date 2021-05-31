using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class UserActivity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public int ActivityId { get; set; }
        public virtual SGCommandActivity Command { get; set; }
        public DateTime DateTriggered { get; set; }
        public bool IsAdmin { get; set; }
    }
}