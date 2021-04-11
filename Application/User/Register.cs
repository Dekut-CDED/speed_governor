using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Application.validators;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.User
{
    public class Register
    {
        public class Command : IRequest<AuthenticationResult>
        {
            public string Lastname { get; set; }
            public string FirstName { get; set; }
            public string PhoneNumber { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }


        }

        public class CommandValidotor : AbstractValidator<Command>
        {
            public CommandValidotor()
            {
                RuleFor(x => x.UserName).NotEmpty();
                RuleFor(x => x.FirstName).NotEmpty();
                RuleFor(x => x.Lastname).NotEmpty();
                RuleFor(x => x.PhoneNumber).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password);

            }
        }

        public class Handler : IRequestHandler<Command, AuthenticationResult>
        {
            private readonly DataContext _context;
            private readonly RoleManager<IdentityRole> roleManager;
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator jwtGeneratorI;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUrlHelper __urlhelper;
            private readonly IHttpContextAccessor __httpcontextAccessor;
            private readonly IEmailSender _emailsender;

            public Handler(DataContext context, IEmailSender emailsender, IHttpContextAccessor _httpcontextAccessor, IUrlHelper _urlhelper, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, IJwtGenerator jwtGenerator)
            {
                this._emailsender = emailsender;
                this.__httpcontextAccessor = _httpcontextAccessor;
                this.__urlhelper = _urlhelper;
                _context = context;
                this.roleManager = roleManager;
                _userManager = userManager;
                this.jwtGeneratorI = jwtGenerator;
                _jwtGenerator = jwtGenerator;
            }
            //Mediator unit is an empty unit/object
            public async Task<AuthenticationResult> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _context.Users.AnyAsync(x => x.Email == request.Email)) throw new RestException(HttpStatusCode.BadRequest, new { error = "Email Exists try another one" });
                // handler logic
                if (await _context.Users.AnyAsync(x => x.UserName == request.UserName)) throw new RestException(HttpStatusCode.BadRequest, new { error = "Username already exist" });


                var user = new AppUser
                {
                    Email = request.Email,
                    UserName = request.UserName,
                    FirstName = request.FirstName,
                    LastName = request.Lastname,
                    PhoneNumber = request.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    return new AuthenticationResult
                    {
                        Errors = result.Errors.Select(x => x.Description)
                    };

                }
                var roleexist = await roleManager.RoleExistsAsync(request.Role);

                if (roleexist)
                {
                    await _userManager.AddToRoleAsync(user, request.Role);
                }


                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var confirmationLink = __urlhelper.Page("/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: __httpcontextAccessor.HttpContext.Request.Scheme);

                await _emailsender.SendEmailAsync(user.Email, "Confirm your email",
                   $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>clicking here</a>.");

                return new AuthenticationResult
                {
                    Success = true,
                    Info = "Please Check your Email to Confirm It"
                };

            }

        }
    }
}
