using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.User
{
    using AutoMapper;
    using Domain;
    using Domain.Dto;
    public class MySpeedGovernors
    {
        public class Query : IRequest<List<SpeedGovernorDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<SpeedGovernorDto>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly UserManager<AppUser> _userManager;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IUserAccessor userAccessor, UserManager<AppUser> userManager, IMapper mapper)
            {
                _userManager = userManager;
                _mapper = mapper;
                _userAccessor = userAccessor;
                _context = context;
            }
            public async Task<List<SpeedGovernorDto>> Handle(Query request,
                CancellationToken cancellationToken)
            {
                // Handler logic goes here
                var user = await _userManager.FindByIdAsync(_userAccessor.GetCurrentUsername());

                if (user == null) throw new RestException(System.Net.HttpStatusCode.Unauthorized, new { Error = "Please use the bearer token" });

                var speedgovernors = await _context.SpeedGovernors.Where(s => s.Owner.Email == user.Email).ToListAsync();

                foreach (var item in speedgovernors)
                {
                    Console.WriteLine(item);
                }
                return _mapper.Map<List<SpeedGovernor>, List<SpeedGovernorDto>>(speedgovernors);
                // Handler logic goes here
            }
        }

    }
}
