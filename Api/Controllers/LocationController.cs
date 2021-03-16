using System.Collections.Generic;
using System.Threading.Tasks;
using Application;
using Application.Location;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class LocationController: BaseController
    {
        [HttpGet("all")]
        public async Task<ActionResult<List<LocationDto>>> GetAllLocations() {
            return await Mediator.Send(new Lists.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<LocationDto>>> GetAllLocations(string id) {
            return await Mediator.Send(new SpeedgovernorLocation.Query(){Id = id});
        }
    }
}