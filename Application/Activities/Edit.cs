using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Edit
    {
    // public class Command : IRequest
    // {
    //     public Guid Id { get; set; }
    //     public string Title { get; set; }
    //     public string Description { get; set; }
    //     public DateTime? Date { get; set; }
    // }

    // public class CommandValidator : AbstractValidator<Command>
    // {
    //     public CommandValidator()
    //     {
    //         RuleFor(x => x.Title).NotEmpty();
    //         RuleFor(x => x.Description).NotEmpty();
    //         RuleFor(x => x.Date).NotEmpty();
    //     }
    // }
    // public class Handler : IRequestHandler<Command>
    // {
    //     private readonly DataContext _context;
    //     public Handler(DataContext context)
    //     {
    //         this._context = context;

    //     }
    //     //Mediator unit is an empty unit/object
    //     public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
    //     {
    //             // var activity = await _context.Activities.FindAsync(request.Id);
                
    //             if(activity == null) {
    //                 throw new Exception(
    //                     "Could not find activity");
    //             }

    //             activity.Title = request.Title ?? activity.Title;
    //             activity.Description = request.Description ?? activity.Description;
    //             activity.Date = request.Date ?? activity.Date;

    //             // handler logic
    //             var success = await _context.SaveChangesAsync() > 0;

    //           if(success) return Unit.Value;
    //           throw new Exception("Prolem saving Changes");
    //     }
    // }
        
    }
}