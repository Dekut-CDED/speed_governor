using Persistence;
using location = Domain.Location;
namespace Application.Interfaces
{
    public class LocationRepository : Repository<location>, ILocation
    {
        private readonly DataContext context;
        public LocationRepository(DataContext context) : base(context)
        {
            this.context = context;
        }
    }
}