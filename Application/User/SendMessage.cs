using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using MediatR;
using Persistence;

namespace Application.User
{
    public class SendMessage
    {
        public class Command : IRequest
        {
            public string SpeedGovernorID { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            
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

              var sms = await _sendMessage.SendMessage(request.Phone,request.Name);
              // User user accessor to get the user phone number.
              // check the role of authorized user.
                
              //var activity = new UserActivity()
                //foreach (var res in sms["SMSMessageData"]["Recipients"])
                //{
                //    var number = res["number"];
                //    var status = res["status"];
                //    var messageId = res["messageId"];
                //    var cost = res["cost"];
                //}
                // handler logic
               // var success = await _context.SaveChangesAsync() > 0;

                if (sms) return Unit.Value;

                throw new Exception("Prolem saving Changes");
            }
        }

    }
}
