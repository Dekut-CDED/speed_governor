using System.Collections.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using MediatR;
using Persistence;
using Domain;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Application.User
{
    public class MySpeedGovernors
    {
        public class Query : IRequest<List<SpeedGovernor>>
        {
        }

        public class Handler : IRequestHandler<Query, List<SpeedGovernor>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly UserManager<AppUser> _userManager;
            public Handler(DataContext context, IUserAccessor userAccessor, UserManager<AppUser> userManager)
            {
                this._userManager = userManager;
                this._userAccessor = userAccessor;
                _context = context;
            }
            public async Task<List<SpeedGovernor>> Handle(Query request,
                CancellationToken cancellationToken)
            {
                // Handler logic goes here
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                var speedgovernors = _context.SpeedGovernors.Where(s => s.Owner == user).ToList();

                return speedgovernors;
                // Handler logic goes here
            }
        }

    }
}