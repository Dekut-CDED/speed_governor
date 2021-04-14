using System.Collections.Generic;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Api.Admin.SpeedGovernors.Profile

{
    public class IndexModel : PageModel
    {
        private readonly IUnitofWork unitofWork;

        public IndexModel(IUnitofWork unitofWork)
        {
            this.unitofWork = unitofWork;
        }

        public Domain.SpeedGovernor SpeedGovernor { get; set; }
        public IEnumerable<Location> Locations { get; set; }
        public void OnGet(string id)
        {
            SpeedGovernor = new SpeedGovernor();
            Locations = new List<Location>();
            if (id != null)
            {
                SpeedGovernor = unitofWork.SpeedGovernor.GetFirstOrDefault(s => s.Id == id, "Owner");
                Locations = unitofWork.Location.GetAll(s => s.SpeedGovId == id);
            }
        }
    }
}
