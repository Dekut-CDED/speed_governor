using Persistence;
using speedGovernor = Domain.SpeedGovernor;
namespace Application.Interfaces
{
    public class SpeedGovernorRepository : Repository<speedGovernor>, ISpeedGovernor
    {
        private readonly DataContext _context;
        public SpeedGovernorRepository(DataContext context) : base(context)
        {
            this._context = context;
        }

        public void Update(speedGovernor gov)
        {
            throw new System.NotImplementedException();
        }
    }
}