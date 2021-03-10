namespace Domain
{
    public class Location
    {
        public string Id { get; set; }
        public string  Time { get; set; }
        public string Speed { get; set; }
        public double Latitude { get; set; }
        public double Long { get; set; }
        public string GpsCourse { get; set; }
        public string SpeedSignalStatus { get; set; }
        public string EngineON { get; set; }
        public virtual SpeedGovernor SpeedGovernor { get; set; }
    }
}
