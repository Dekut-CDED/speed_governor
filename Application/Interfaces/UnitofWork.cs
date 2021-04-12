using Domain;
using Microsoft.AspNetCore.Identity;
using Persistence;

namespace Application.Interfaces
{
    public class UnitofWork : IUnitofWork
    {
        private readonly DataContext db;
        private readonly UserManager<AppUser> usermanager;
        private readonly SignInManager<AppUser> signinmanager;
        private readonly IJwtGenerator jwtgenerator;
        private readonly IUserAccessor userAccessor;
        private readonly RoleManager<IdentityRole> roleManage;
        public UnitofWork(DataContext db,
            UserManager<AppUser> usermanager,
            SignInManager<AppUser> signinmanager,
            IJwtGenerator jwtgenerator,
            IUserAccessor userAccessor,
            RoleManager<IdentityRole> roleManage)
        {
            this.db = db;
            this.usermanager = usermanager;
            this.signinmanager = signinmanager;
            this.jwtgenerator = jwtgenerator;
            this.userAccessor = userAccessor;
            this.roleManage = roleManage;

            AppUser = new AppUserRepository(db);
            Location = new LocationRepository(db);

        }
        public IAppUser AppUser { get; private set; }
        public ILocation Location { get; private set; }
        public void Save()
        {
            db.SaveChangesAsync();
        }


    }
}