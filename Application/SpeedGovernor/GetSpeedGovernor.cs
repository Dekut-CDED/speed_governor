using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Persistence;
using AutoMapper;
using Application.User;
using Domain;
using speedGovernor = Domain.SpeedGovernor;
using Application.Interfaces;
using Application.Errors;
using System.Net;
using Microsoft.AspNetCore.Identity;

namespace Application.SpeedGovernor
{
    public class GetSpeedGovernor
    {

         public class Query : IRequest<SpeedGovernorDto>
         {
           public int Id { get; set; }
         }

         public class Handler : IRequestHandler<Query, SpeedGovernorDto>
         {
             private readonly DataContext _context;
            private readonly UserManager<AppUser> _userManager;
            private readonly IUserAccessor _userAccessor;
            private readonly IMapper _mapper;
             public Handler(DataContext context,UserManager<AppUser> userManager, IUserAccessor userAccessor ,IMapper mapper)
             {
                 _mapper = mapper;
                 _context = context;
                _userManager = userManager;
                _userAccessor = userAccessor;
            }
             public async Task<SpeedGovernorDto> Handle(Query request,
                   CancellationToken cancellationToken)
             {

                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                if(user == null) {
                   throw new RestException(HttpStatusCode.Unauthorized, new { User = "Not authorize to access the code" });
                }

                 var speedgovernor = await _context.SpeedGovernors.FindAsync(request.Id);

               return _mapper.Map<speedGovernor, SpeedGovernorDto>(speedgovernor);
             }
         }

    }
}
