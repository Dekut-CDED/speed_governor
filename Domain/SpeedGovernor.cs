using System.Collections.Generic;
namespace Domain
{
    public class SpeedGovernor
    {
         public string Id { get; set; }
         public string Imei { get; set; }
         public string PlateNumber { get; set; }
         public string Phone { get; set; }
         public string Fuellevel {get; set; }
         public string Vibrations { get; set; }
     
     
         public virtual AppUser Owner { get; set; }
         public virtual ICollection<Location> Locations { get; set; }

         // vibration
         // fuelsize

    }

}