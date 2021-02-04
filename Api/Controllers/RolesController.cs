using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Errors;
using Application.Roles;
using Application.User;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Api.Controllers
{
    //[Authorize(Roles = Role.Admin)]
    public class RolesController: Controller {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(DataContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }        
        //[Authorize(Roles = Role.Admin)]
        
        [HttpGet("getall")]
        public async Task<ActionResult<List<UsersRole>>> GetAllRoles()
        {
            var roles =await  _context.Roles.ToListAsync();
            var newroles = roles.Select(x => new UsersRole()
            {
              Name =x.Name
            });
            return newroles.ToList();
        }
        
        [HttpPost("createrole")]
        public async  Task<ActionResult<UsersRole>> CreateRole(UsersRole role)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(){Name = role.Name});
            if (result.Succeeded)
            {
                return new UsersRole()
                {
                    Name = role.Name
                };
            }

            throw new RestException(HttpStatusCode.NotImplemented, new {errors = "Cound not create the role"});
        }
        
        
        
    }
    
}
