using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence;

namespace Application.User
{
    public class CurrentUser
    {
        public class Query : IRequest<object>
        {

        }

        public class Handler : IRequestHandler<Query, object>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUserAccessor _userAccessor;
            public Handler(UserManager<AppUser> userManager, IJwtGenerator jwtGenerator, IUserAccessor userAccessor)
            {
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
                _userAccessor = userAccessor;
            }
            public async Task<object> Handle(Query request,
                  CancellationToken cancellationToken)
            {
                // Handler logic goes here
                var user = await _userManager.FindByIdAsync(_userAccessor.GetCurrentUsername());

                return new
                {
                    Email = user.Email,
                    Username = user.UserName,
                    Token = _jwtGenerator.createToken(user),
                };
            }
        }

    }
}