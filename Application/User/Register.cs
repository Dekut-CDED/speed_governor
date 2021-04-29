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
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using Newtonsoft.Json;
using System.Collections.Generic;
using AutoMapper;

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

            private IDistributedCache _cache;
            private IHostApplicationLifetime _lifetime;
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("41.89.227.168");
            private IMapper _mapper;

            public Handler(DataContext context, IEmailSender emailsender,
                IHttpContextAccessor _httpcontextAccessor, IUrlHelper _urlhelper,
                RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager,
                IJwtGenerator jwtGenerator, IHostApplicationLifetime lifetime, IDistributedCache cache,
                IMapper mapper)
            {
                this._emailsender = emailsender;
                this.__httpcontextAccessor = _httpcontextAccessor;
                this.__urlhelper = _urlhelper;
                _context = context;
                this.roleManager = roleManager;
                _userManager = userManager;
                this.jwtGeneratorI = jwtGenerator;
                _jwtGenerator = jwtGenerator;
                this._cache = cache;
                this._lifetime = lifetime;
                this._mapper = mapper;
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

                #region Redis Test
                // Test the Redis Thing Here

                byte[] cachedUsers = await _cache.GetAsync("cachedUsers");
                List<UserCacheDto> users = new List<UserCacheDto>();
                var userDto = _mapper.Map<AppUser, UserCacheDto>(user);

                if (cachedUsers != null)
                {
                    var appUsersString = Encoding.UTF8.GetString(cachedUsers);
                    var appUsers = JsonConvert.DeserializeObject<List<UserCacheDto>>(appUsersString);
                    appUsers.Add(userDto);
                    users.AddRange(appUsers);
                }

                else
                {
                    users.Add(userDto);
                }

                _lifetime.ApplicationStarted.Register(() =>
                {

                    var usersString = JsonConvert.SerializeObject(users);
                    cachedUsers = Encoding.UTF8.GetBytes(usersString);
                    var options = new DistributedCacheEntryOptions().
                                   SetSlidingExpiration(TimeSpan.FromDays(365));
                    _cache.Set("cachedUsers", cachedUsers, options);
                }
                   );

                #endregion

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
