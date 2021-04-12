using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Api.Admin.SpeedGovernors.Profile

{
    public class ProfileModel : PageModel
    {
        private readonly IUnitofWork unitofWork;

        public ProfileModel(IUnitofWork unitofWork)
        {
            this.unitofWork = unitofWork;
        }
        public void OnGet()
        {
        }
    }
}
