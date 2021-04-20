using System.Data.Common;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Application.User;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence;

namespace Application.SpeedGovernor
{
    public class DeleteSpeedGovernor
    {

        public class Query : IRequest<Unit>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Unit>
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
            public async Task<Unit> Handle(Query request,
                  CancellationToken cancellationToken)
            {

                var user = await _userManager.FindByIdAsync(_userAccessor.GetCurrentUsername());

                if (user == null)
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { User = "Not authorize to access the code" });
                }

                var speedgovernor = await _context.SpeedGovernors.FindAsync(request.Id);
                _context.SpeedGovernors.Remove(speedgovernor);
                var result = await _context.SaveChangesAsync();
                return new Unit();
            }
        }

    }
}