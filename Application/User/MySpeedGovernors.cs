using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using MediatR;
using Persistence;
using speedGovernor = Domain.SpeedGovernor;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Domain;

namespace Application.User
{
    public class MySpeedGovernors
    {
        public class Query : IRequest<List<SpeedGovernorDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<SpeedGovernorDto>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly UserManager<AppUser> _userManager;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IUserAccessor userAccessor, UserManager<AppUser> userManager, IMapper mapper)
            {
                _userManager = userManager;
                _mapper = mapper;
                _userAccessor = userAccessor;
                _context = context;
            }
            public async Task<List<SpeedGovernorDto>> Handle(Query request,
                CancellationToken cancellationToken)
            {
                // Handler logic goes here
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                var speedgovernors = _context.SpeedGovernors.Where(s => s.Owner.Email == user.Email).ToList();

                return _mapper.Map<List<speedGovernor>, List<SpeedGovernorDto>>(speedgovernors);
                // Handler logic goes here
            }
        }

    }
}
