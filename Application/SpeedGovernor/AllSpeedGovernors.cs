using System.Collections.Generic;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Application.User;
using Domain;
using speedGovernor = Domain.SpeedGovernor;
using Application.Interfaces;
using Application.Errors;
using System.Net;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Application.SpeedGovernor
{
    public class AllSpeedGovernors
    {

        public class Query : IRequest<List<SpeedGovernorDto>>
        {

        }

        public class Handler : IRequestHandler<Query, List<SpeedGovernorDto>>
        {
            private readonly DataContext _context;
            private readonly UserManager<AppUser> _userManager;
            private readonly IUserAccessor _userAccessor;
            private readonly IMapper _mapper;
            private readonly IUnitofWork _unitofwork;

            public Handler(DataContext context, UserManager<AppUser> userManager, IUserAccessor userAccessor, IMapper mapper, IUnitofWork unitofwork)
            {
                _mapper = mapper;
                _unitofwork = unitofwork;
                _context = context;
                _userManager = userManager;
                _userAccessor = userAccessor;
            }
            public async Task<List<SpeedGovernorDto>> Handle(Query request,
                  CancellationToken cancellationToken)
            {

                var user = await _userManager.FindByIdAsync(_userAccessor.GetCurrentUsername());

                if (user == null)
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { User = "Not authorize to access the code" });
                }

                var speedgovernors = _unitofwork.SpeedGovernor.GetAll(null,null,"Owner").ToList();

                return _mapper.Map<List<speedGovernor>, List<SpeedGovernorDto>>(speedgovernors);
            }
        }
    }
}
