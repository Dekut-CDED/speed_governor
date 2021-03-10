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
        public async Task<ActionResult<User>> login(Login.Query query){
            return await Mediator.Send(query);
        }
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(Register.Command command){
            return await Mediator.Send(command);
        }
        [HttpGet]
        public async Task<ActionResult<User>> CurrentUser(){
            return await Mediator.Send(new CurrentUser.Query());
        }

        [HttpPost("sendmessage")]
        public async Task<Unit> SendMessage(SendMessage.Command command){
            return await Mediator.Send(command);
        }

        [HttpPost("myspeedgovernors")]
        public async Task<List<SpeedGovernorDto>> MySpeedGovernor(MySpeedGovernors.Query query)
        {
            return await Mediator.Send(query);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(string id){
            return await Mediator.Send(new GetUserById.Query() {Id = id}
                    );
        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet("all")]
        public async Task<ActionResult<List<User>>> GetAllUsers(){
            return await Mediator.Send(new GetUsers.Query() 
                    );
        }
        
        [HttpPost("addrole/")]
        public async  Task<ActionResult> AddUserRole(RoleViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.UserEmail);
           var result= await  _userManager.AddToRoleAsync(user, model.Role);
           return Ok(result);
        }
        
        [HttpDelete("removefromRole")]
        public async  Task<ActionResult> RemoveUserFromRole(RoleViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.UserEmail);
           var result= await  _userManager.RemoveFromRoleAsync(user, model.Role);
           return Ok(result);
        }
    }
    public class RoleViewModel {
        public string UserEmail{ get; set; }
        public string Role { get; set; }
        }
    }
