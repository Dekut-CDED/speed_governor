using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;
using Application.User;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [AllowAnonymous]
    [Produces("application/json")]
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

        [HttpPost("sendmessage")]
        public async Task<Unit> SendMessage(SendMessage.Command command){
            return await Mediator.Send(command);
        }

        [HttpPost("myspeedgovernors")]
        public async Task<List<SpeedGovernorDto>> MySpeedGovernor(MySpeedGovernors.Query query)
        {
            return await Mediator.Send(query);
        }
    }
}
