using System.Collections.Generic;
using System.Threading.Tasks;
using Application.User;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace Api.Controllers
{

    [AllowAnonymous]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class UserController : BaseController
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UserController(DataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpPost("login")]
        public async Task<ActionResult<object>> login(Login.Query query)
        {
            return await Mediator.Send(query);
        }
        [HttpPost("register")]
        public async Task<ActionResult<AuthenticationResult>> Register(Register.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet("/confirmEmail/{id}/{token}")]
        public async Task<ActionResult<object>> ConfirmEmail(string id, string token)
        {
            return await Mediator.Send(new ConfirmEmail.Query() { userId = id, token = token });
        }
        [HttpGet]
        public async Task<ActionResult<object>> CurrentUser()
        {
            return await Mediator.Send(new CurrentUser.Query());
        }

        [HttpPost("sendmessage")]
        public async Task<Unit> SendMessage(SendMessage.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("myspeedgovernors")]
        public async Task<List<SpeedGovernorDto>> MySpeedGovernor(MySpeedGovernors.Query query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(string id)
        {
            return await Mediator.Send(new GetUserById.Query() { Id = id }
                    );
        }

        [Authorize(Roles = "CdedAdmin")]
        [HttpGet("all")]
        public async Task<ActionResult> GetAllUsers()
        {
            var result = await Mediator.Send(new GetUsers.Query()
                    );

            return Json(new { data = result });
        }

        [HttpPost("addusertorole/")]
        public async Task<ActionResult> AddUserRole(RoleViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.UserEmail);
            var result = await _userManager.AddToRoleAsync(user, model.Role);
            return Ok(result);
        }

    }
}
