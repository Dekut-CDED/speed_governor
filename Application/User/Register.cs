using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Application.validators;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.User
{
    public class Register
    {
        public class Command : IRequest<User>
        {
            public string DisplayName { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Imei { get; set; }
            public string Role {get; set;}
            
        }

        public class CommandValidotor : AbstractValidator<Command>
        {
            public CommandValidotor()
            {
                RuleFor(x => x.DisplayName).NotEmpty();
                RuleFor(x => x.UserName).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).Password();
                RuleFor(x => x.Role).NotEmpty();

            }
        }

        public class Handler : IRequestHandler<Command, User>
        {
            private readonly DataContext _context;
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;

            public Handler(DataContext context, UserManager<AppUser> userManager, IJwtGenerator jwtGenerator)
            {
                _context = context;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
            }
            //Mediator unit is an empty unit/object
            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                if(await _context.Users.AnyAsync(x => x.Email ==request.Email)) throw new RestException(HttpStatusCode.BadRequest, new { error = "Email Exists try another one" });
                // handler logic
                if(await _context.Users.AnyAsync(x => x.UserName ==request.UserName)) throw new RestException(HttpStatusCode.BadRequest, new { error = "Username already exist" });


                var user = new AppUser
                {
                    DisplayName = request.DisplayName,
                    Email = request.Email,
                    UserName = request.UserName,
                    Imei = request.Imei
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                
                if(result.Succeeded) return new User
                {
                    Email = user.Email,
                    Token = _jwtGenerator.createToken(user),
                    Username = user.UserName,
            };
                throw new RestException(HttpStatusCode.BadRequest, new {error = "Failed please Try again"});
            }
        }
        
    }
}
