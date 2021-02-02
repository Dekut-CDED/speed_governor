using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using MediatR;
using Persistence;
using Domain;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Net;
using AutoMapper;
using Application.Errors;
using speedGovernor = Domain.SpeedGovernor;


namespace Application.User
{
    public class LatestLocation
    {
        public class Query : IRequest<LocationDto>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, LocationDto>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly UserManager<AppUser> _userManager;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IUserAccessor userAccessor, UserManager<AppUser> userManager, IMapper mapper)
            {
                _userManager = userManager;
                _userAccessor = userAccessor;
                _context = context;
                _mapper = mapper;
            }
            public async Task<LocationDto> Handle(Query request,
                CancellationToken cancellationToken)
            {
                // Handler logic goes here
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());
                if (user == null)
                {
                    throw new RestException(HttpStatusCode.Unauthorized, "User not authorized");
                }

                var speedgovernor = await _context.SpeedGovernors.FindAsync(request.Id);

                if (speedgovernor == null)
                    throw new RestException(HttpStatusCode.NotFound, "Speed Governor does not exist");

                var location = _context.Locations.Where(s => s.SpeedGovernor.Imei == speedgovernor.Imei).FirstOrDefault();

                return _mapper.Map<Location, LocationDto>(location);
                // Handler logic goes here
            }
        }

    }
}
