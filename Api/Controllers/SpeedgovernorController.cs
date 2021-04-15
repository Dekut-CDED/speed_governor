using System.Threading.Tasks;
using Application;
using Application.SpeedGovernor;
using Application.User;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

    [ApiVersion("1.0")]
    public class SpeedGovernorController : BaseController
    {
        // Get the current location     
        [HttpGet("{id}/location")]
        public async Task<ActionResult<LocationDto>> GetLatestLocation(string id)
        {
            return await Mediator.Send(new LatestLocation.Query() { Id = id });
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetSpeedGovornors()
        {
            var speedgovs = await Mediator.Send(new AllSpeedGovernors.Query());
            return Json(new { data = speedgovs });
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<SpeedGovernorDto>> GetSpeedGovornor(string id)
        {
            return await Mediator.Send(new GetSpeedGovernor.Query() { Id = id });
        }

        /* [HttpDelete("{id}")]
         public async Task<ActionResult<Unit>> DeleteSpeedGovornor(string id){
             return await Mediator.Send(new DeleteSpeedGovernor.Query(){Id=id});
         }*/

    }
}
