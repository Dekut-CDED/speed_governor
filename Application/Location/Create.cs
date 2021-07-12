using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence;

namespace Application.Location
{
    using Domain;
    public class Create
    {


        public class Command : IRequest<Location>
        {
            public Location Location { get; set; }
        }

        public class Handler : IRequestHandler<Command, Location>
        {
            private readonly DataContext context;
            private readonly IUserAccessor userAccessor;
            private readonly UserManager<AppUser> userManager;
            private readonly IMapper mapper;
            private readonly IUnitofWork unitofwork;

            public Handler(DataContext context, IUserAccessor userAccessor, UserManager<AppUser> userManager, IMapper mapper, IUnitofWork unitofwork)
            {
                this.unitofwork = unitofwork;
                this.context = context;
                this.userAccessor = userAccessor;
                this.userManager = userManager;
                this.mapper = mapper;
            }
            public Task<Location> Handle(Command request, CancellationToken cancellationToken)
            {
               unitofwork.Location.Add(request.Location);
               unitofwork.Save();
               return Task.FromResult(request.Location);
            }
        }



    }
}