using System.Collections.Generic;
using System.Threading.Tasks;
using Application.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

    [AllowAnonymous]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class UserController : BaseController
    {
        [HttpPost("login")]
        public async Task<ActionResult<User>> login(Login.Query query){
            return await Mediator.Send(query);
        }
        [HttpPost("register")]
        public async Task<ActionResult<User>> login(Register.Command command){
            return await Mediator.Send(command);
        }
        [HttpGet]
        public async Task<ActionResult<User>> CurrentUser(){
            return await Mediator.Send(new CurrentUser.Query());
        }

        [HttpPost("command")]
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

        [HttpGet("all")]
        public async Task<ActionResult<List<User>>> GetAllUsers(){
            return await Mediator.Send(new GetUsers.Query() 
                    );
        }
    }
}
