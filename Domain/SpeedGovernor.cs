using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class SpeedGovernor
    {

        [Key]
        public string Imei { get; set; }
        public string PlateNumber { get; set; }
        public string Phone { get; set; }
        public string Fuellevel { get; set; }
        public string Vibrations { get; set; }


        public string OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public virtual AppUser Owner { get; set; }
        public virtual ICollection<Location> Locations { get; set; }

        // vibration
        // fuelsize

    }

}
