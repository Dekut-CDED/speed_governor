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

namespace Application.User
{
    public class GetUsers
    {
    public class Query : IRequest<List<User>>
    {
    }

    public class Handler : IRequestHandler<Query, List<User>>
    {
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUserAccessor _userAccessor;
            private readonly IMapper _mapper;

            public Handler(UserManager<AppUser> userManager, IJwtGenerator jwtGenerator, IUserAccessor userAccessor, IMapper mapper)
        {
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
                _userAccessor = userAccessor;
                _mapper = mapper;
            }
        public async Task<List<User>> Handle(Query request,
              CancellationToken cancellationToken)
        {
                // Handler logic goes here
                //TODO CHECK FOR THE ROLE
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                if(user == null) {
                   throw new RestException(HttpStatusCode.Unauthorized, new { User = "Not authorize to access the code" });
                }

                var foundusers = await _userManager.Users.ToListAsync();

                return _mapper.Map<List<AppUser>,List<User>>(foundusers);
            }

    }
        
    }
}