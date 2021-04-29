using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.User
{
    public class ConfirmEmail
    {
        public class Query : IRequest<object>
        {
            public string userId { get; set; }
            public string token { get; set; }
        }

        public class Handler : IRequestHandler<Query, object>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUserAccessor _userAccessor;
            private readonly IEmailSender _emailService;
            public Handler(UserManager<AppUser> userManager, IEmailSender emailService, IJwtGenerator jwtGenerator, IUserAccessor userAccessor)
            {
                this._emailService = emailService;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
                _userAccessor = userAccessor;
            }
            public async Task<object> Handle(Query request,
                  CancellationToken cancellationToken)
            {
                // Handler logic goes here
                var user = await _userManager.FindByIdAsync(_userAccessor.GetCurrentUsername());

                if (request.userId == null || request.token == null)
                {
                    throw new RestException(System.Net.HttpStatusCode.BadRequest, new { error = "Provide the UserId and Token" });
                }

                var user2 = await _userManager.FindByIdAsync(request.userId);
                if (user2 == null)
                {
                    throw new RestException(System.Net.HttpStatusCode.BadRequest, new { error = "UserId does not exist" });
                }

                var result = await _userManager.ConfirmEmailAsync(user, request.token);

                if (result.Succeeded)
                {
                    await _emailService.SendEmailAsync(user.Email, "CDED - Successfully Registered", "Congratulations,\n You have successfully activated your account!\n " +
                 "Welcome to the dark side.");
                }

                return Task.FromResult(new { success = "Account Confirmed" });

            }
        }
    }
}