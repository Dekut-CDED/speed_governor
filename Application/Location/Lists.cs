using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using location = Domain.Location;

namespace Application.Location
{
    public class Lists
    {

        public class Query : IRequest<List<LocationDto>>
        {

        }

        public class Handler : IRequestHandler<Query, List<LocationDto>>
        {
            private readonly DataContext _context;
            private readonly UserManager<AppUser> _userManager;
            private readonly IUserAccessor _userAccessor;
            private readonly IMapper _mapper;
            public Handler(DataContext context, UserManager<AppUser> userManager, IUserAccessor userAccessor, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
                _userManager = userManager;
                _userAccessor = userAccessor;
            }
            public async Task<List<LocationDto>> Handle(Query request,
                  CancellationToken cancellationToken)
            {

                var user = await _userManager.FindByIdAsync(_userAccessor.GetCurrentUsername());

                if (user == null)
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { User = "Not authorize to access the code" });
                }

                var locations = await _context.Locations.ToListAsync();

                //return _mapper.Map<List<speedGovernor>, List<SpeedGovernorDto>>(speedgovernors);
                return _mapper.Map<List<location>, List<LocationDto>>(locations);
            }
        }

    }
}