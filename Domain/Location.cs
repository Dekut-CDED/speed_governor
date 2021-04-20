using System.ComponentModel.DataAnnotations.Schema;
namespace Domain
{
    public class Location
    {
        public string Id { get; set; }
        public string Time { get; set; }
        public string  Date { get; set; }
        public string Speed { get; set; }
        public double Latitude { get; set; }
        public double Long { get; set; }
        public string GpsCourse { get; set; }
        public string SpeedSignalStatus { get; set; }
        public string EngineON { get; set; }
        public string SpeedGovId { get; set; }
        [ForeignKey("SpeedGovId")]
        public virtual SpeedGovernor SpeedGovernor { get; set; }
    }
}
