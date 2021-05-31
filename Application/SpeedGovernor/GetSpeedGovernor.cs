using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence;

namespace Application.SpeedGovernor
{
    using Domain;
    using Domain.Dto;
    public class GetSpeedGovernor
    {


        public class Query : IRequest<SpeedGovernorDto>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, SpeedGovernorDto>
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
            public async Task<SpeedGovernorDto> Handle(Query request,
                CancellationToken cancellationToken)
            {

                var user = await _userManager.FindByIdAsync(_userAccessor.GetCurrentUsername());

                if (user == null)
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { User = "Not authorize to access the code" });
                }

                var speedgovernor = await _context.SpeedGovernors.FindAsync(request.Id);

                return _mapper.Map<SpeedGovernor, SpeedGovernorDto>(speedgovernor);
            }
        }
    }
}
