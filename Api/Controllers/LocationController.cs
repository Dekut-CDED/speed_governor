using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Interfaces;
using Application.Location;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using location = Domain.Location;

namespace Api.Controllers
{
    public class LocationController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;
        public LocationController(IUnitofWork unitofWork, IMapper mapper)
        {
            this._mapper = mapper;
            this._unitofWork = unitofWork;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllLocations()
        {
            return Json(new { data = _mapper.Map<List<location>, List<LocationDto>>(_unitofWork.Location.GetAll().ToList()) });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAllLocations(string id)
        {
            var locations = _unitofWork.Location.GetAll(l => l.SpeedGovId == id).ToList();
            return Json(new { data = _mapper.Map<List<location>, List<LocationDto>>(locations) });
        }
    }
}