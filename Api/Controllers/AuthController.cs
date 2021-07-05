using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Dto;
using Application.Auth;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Infrastructure.security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]

    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IJwtGenerator tokenGenerator;
        private readonly IMapper mapper;
        private readonly IUserAccessor userAccessor;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtGenerator tokenGenerator, IMapper mapper, IUserAccessor userAccessor)
        {
            this.userAccessor = userAccessor;
            this.tokenGenerator = tokenGenerator;
            this.mapper = mapper;
            this.signInManager = signInManager;
            this.userManager = userManager;

        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<RegisterResult>> Register([FromBody] RegisterDto userreg)
        {
            if (await userManager.Users.AnyAsync(x => x.Email == userreg.Email))
            {
                throw new RestException(HttpStatusCode.Conflict, new { error = "Email Taken" });
            }

            if (await userManager.Users.AnyAsync(x => x.UserName == userreg.UserName))
            {
                throw new RestException(HttpStatusCode.Conflict, new { error = "UserName Taken" });
            }

            var user = new AppUser
            {
                FirstName = userreg.FirstName,
                LastName = userreg.Lastname,
                PhoneNumber = userreg.PhoneNumber,
                UserName = userreg.UserName,
                Email = userreg.Email,
            };

            var result = await userManager.CreateAsync(user, userreg.Password);
            if (!result.Succeeded)
            {
                throw new RestException(HttpStatusCode.Conflict, new { error = result.Errors });
            }

            return new RegisterResult
            {
                Id = user.Id,
                Token = tokenGenerator.createToken(user),
                UserName = user.UserName
            };
        }
        [HttpPost("login")]
        public async Task<ActionResult<RegisterResult>> login([FromBody] LoginDto loginuser)
        {
            var user = await userManager.FindByEmailAsync(loginuser.Email);
            if (user == null) return Unauthorized();

            var result = await signInManager.CheckPasswordSignInAsync(user, loginuser.Password, false);

            if (result.Succeeded)
            {             
            return new RegisterResult{
                     Token = tokenGenerator.createToken(user),
                     UserName = user.UserName,
                     Id = user.Id,
               };
            }
   throw new RestException(HttpStatusCode.Unauthorized, new { error = "Check you Email and Password"});
               
        }
       [Authorize]
       [HttpGet]
        public async Task<ActionResult<UserDto>> CurrentUser()
        {
            var user = await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            return new UserDto{
              FirstName = user.FirstName,
              LastName = user.LastName,
              PhoneNumber = user.UserName,
              Email = user.Email,
            };
        }
    }
}