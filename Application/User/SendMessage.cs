using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using MediatR;
using Persistence;

namespace Application.User
{
    public class SendMessage
    {
        public class Command : IRequest
        {
            public string Name { get; set; }
            public string phone { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IMessage _sendMessage;
            public Handler(DataContext context, IMessage sendMessage)
            {
                this._sendMessage = sendMessage;
                this._context = context;

            }
            //Mediator unit is an empty unit/object
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {

                var messageStatus = _sendMessage.SendMessage(request.Name,request.phone);

                // handler logic
                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Prolem saving Changes");
            }
        }

    }
}