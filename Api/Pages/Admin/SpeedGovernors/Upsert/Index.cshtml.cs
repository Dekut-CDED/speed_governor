using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Api.Admin.SpeedGovernors.Upsert
{
    public class IndexModel : PageModel
    {
        private readonly IUnitofWork _unitofwork;
        private readonly IMapper mapper;

        public IndexModel(IUnitofWork unitofwork, IMapper mapper)
        {
            this._unitofwork = unitofwork;
            this.mapper = mapper;
        }
        [BindProperty]
        public InputModel Input { get; set; }


        public class InputModel
        {

            [Required]
            [StringLength(10)]
            [DataType(DataType.Text)]

            public string PhoneNumber { get; set; }

            [Required]
            [StringLength(10)]
            [DataType(DataType.Text)]

            public string PlateNumber { get; set; }
            [Required]
            [StringLength(50)]
            [DataType(DataType.Text)]

            public string Imei { get; set; }

            [Required]
            [StringLength(50)]
            [DataType(DataType.Text)]
            public string OwnerId { get; set; }
        }


        public SpeedGovernor SpeedgovObj { get; set; }
        public List<NameList> UserList { get; set; }
        public IActionResult OnGet(string imei)
        {

            var appUsers = _unitofwork.AppUser.GetAll().ToList();
            UserList = mapper.Map<List<AppUser>, List<NameList>>(appUsers);
            SpeedgovObj = new SpeedGovernor();
            Input = new InputModel();
            if (imei != null)
            {
                SpeedgovObj = _unitofwork.SpeedGovernor.Get(imei);
                if (SpeedgovObj == null)
                {
                    return NotFound();
                }
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            var userId = Request.Form["userlist"];

            SpeedgovObj = new SpeedGovernor();
            if (ModelState.IsValid)
            {
                return Page();
            }

            if (SpeedgovObj.Imei == null)
            {
                var speedGovernor = new SpeedGovernor { Imei = Input.Imei, Phone = Input.PhoneNumber, PlateNumber = Input.PlateNumber, OwnerId = userId };
                _unitofwork.SpeedGovernor.Add(speedGovernor);
            }
            else
            {
                _unitofwork.SpeedGovernor.Update(SpeedgovObj);
            }
            _unitofwork.Save();
            return RedirectToPage("/Admin/SpeedGovernors/Home/Home");
        }
    }
}
