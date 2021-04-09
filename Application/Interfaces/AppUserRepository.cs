using Domain;
using Persistence;

namespace Application.Interfaces
{
    public class AppUserRepository : Repository<AppUser>, IAppUser
    {
        public AppUserRepository(DataContext context) : base(context)
        {
        }
    }
}