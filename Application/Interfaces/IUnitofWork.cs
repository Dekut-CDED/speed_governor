namespace Application.Interfaces
{
    public interface IUnitofWork
    {
        void Save();
        IAppUser AppUser { get; }
        ILocation Location { get; }
        ISpeedGovernor SpeedGovernor { get; }
    }
}