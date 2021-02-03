using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.User
{
    public class GetUserById
    {
    public class Query : IRequest<User>
    {
        public string Id { get; set; }

    }

    public class Handler : IRequestHandler<Query, User>
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
        public async Task<User> Handle(Query request,
              CancellationToken cancellationToken)
        {
                // Handler logic goes here
                //TODO CHECK FOR THE ROLE
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                if(user == null) {
                   throw new RestException(HttpStatusCode.Unauthorized, new { User = "Not authorize to access the code" });
                }

                var founduser = await _userManager.FindByIdAsync(request.Id);

                return _mapper.Map<AppUser,User>(founduser);

            }

    }
        
    }
}
