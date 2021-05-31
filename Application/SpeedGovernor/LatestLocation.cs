using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.User
{

    using Domain;
    public class LatestLocation
    {
        public class Query : IRequest<LocationDto>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, LocationDto>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly UserManager<AppUser> _userManager;
            private readonly IMapper _mapper;
            private readonly IUnitofWork _unitofWork;
            public Handler(IUnitofWork unitofWork, DataContext context, IUserAccessor userAccessor, UserManager<AppUser> userManager, IMapper mapper)
            {
                this._unitofWork = unitofWork;
                _userManager = userManager;
                _userAccessor = userAccessor;
                _context = context;
                _mapper = mapper;

            }
            public async Task<LocationDto> Handle(Query request,
                CancellationToken cancellationToken)
            {
                // Handler logic goes here
                var user = await _userManager.FindByIdAsync(_userAccessor.GetCurrentUsername());
                if (user == null)
                {
                    throw new RestException(HttpStatusCode.Unauthorized, "User not authorized");
                }

                var speedgovernor = await _context.SpeedGovernors.FindAsync(request.Id);

                if (speedgovernor == null)
                    throw new RestException(HttpStatusCode.NotFound, "Speed Governor does not exist");

                // Try to use Redis here to get location
                var location = await _context.Locations.Where(s => s.SpeedGovernor.Imei == speedgovernor.Imei).SingleOrDefaultAsync();

                return _mapper.Map<Location, LocationDto>(location);
                // Handler logic goes here
            }
        }

    }
}
