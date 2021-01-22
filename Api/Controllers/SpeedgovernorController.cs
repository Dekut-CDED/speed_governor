using System.Threading.Tasks;
using Application;
using Application.User;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

   [ApiVersion("1.0")]
    public class SpeedGovernorController : BaseController
    {
        // Get the current location     
        [HttpGet("{id}")]
        public async Task<ActionResult<LocationDto>> GetLatestLocation(int id){
            return await Mediator.Send(new LatestLocation.Query(){Id=id});
        }

    }
}
