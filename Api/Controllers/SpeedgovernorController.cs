using System.Collections.Generic;
using System.Threading.Tasks;
using Application;
using Application.SpeedGovernor;
using Application.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

   [ApiVersion("1.0")]
    public class SpeedGovernorController : BaseController
    {
        // Get the current location     
        [HttpGet("{id}/location")]
        public async Task<ActionResult<LocationDto>> GetLatestLocation(int id){
            return await Mediator.Send(new LatestLocation.Query(){Id=id});
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<SpeedGovernorDto>>> GetSpeedGovornors(){
            return await Mediator.Send(new AllSpeedGovernors.Query() 
                    );
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<SpeedGovernorDto>> GetSpeedGovornor(int id){
            return await Mediator.Send(new GetSpeedGovernor.Query(){Id=id});
        }

    }
}
