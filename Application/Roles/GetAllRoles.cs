using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Domain;
using Persistence;
using System.Linq;

namespace Application.Roles
{
    public class GetAllRoles
    {
      public class Query : IRequest<List<UsersRole>>
      {

      }

      public class Handler : IRequestHandler<Query, List<UsersRole>>
      {
            private readonly DataContext _context;
            private readonly UserManager<AppUser> usermanager;
            private readonly RoleManager<AppUser> roleManager;

            public Handler(DataContext context, UserManager<AppUser> usermanager, RoleManager<AppUser> roleManager)
        {
            _context = context;
                this.usermanager = usermanager;
                this.roleManager = roleManager;
            }
        public async Task<List<UsersRole>> Handle(Query request, CancellationToken cancellationToken)
        {
           var roles =await _context.Roles.ToListAsync();
           var convertedroles = roles.Select(y => new UsersRole {
                Name = y.Name
               }).ToList();
           return convertedroles;
        }
      }
    }
}
