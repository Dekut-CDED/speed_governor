using speedGovernor = Domain.SpeedGovernor;
namespace Application.Interfaces
{
    public interface ISpeedGovernor : IRepository<speedGovernor>
    {
        void Update(speedGovernor gov);
    }
}