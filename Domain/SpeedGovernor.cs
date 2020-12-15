using System.Collections.Generic;
namespace Domain
{
    public class SpeedGovernor
    {
     public int Id { get; set; }
     public string Imei { get; set; }
     public string PlateNumber { get; set; }
     public string Phone { get; set; }
     public virtual AppUser Owner { get; set; }
     public virtual ICollection<Location> Speeds { get; set; }
    }

}