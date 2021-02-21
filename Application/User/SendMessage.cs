using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Configuration;
using Persistence;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Application.User
{
    public class SendMessage
    {
        public class Command : IRequest
        {
            public string Name { get; set; }
            public string phone { get; set; }
            public  string  Message { get; set; }
        }
       
        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IMessage _sendMessage;
            private  readonly IConfiguration _configuration;
            public Handler(DataContext context, IMessage sendMessage, IConfiguration configuration)
            {
                this._sendMessage = sendMessage;
                this._context = context;
                this._configuration = configuration;

            }
            //Mediator unit is an empty unit/object
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {

                //var sms = await _sendMessage.SendMessage(request.phone,request.Name);
                var twilioSms = await _sendMessage.SendTwilioMessage(request.Name, request.Message);

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
                #region twilio
                // Get credentials for Twilio
                var twilioCredentials = _configuration.GetSection("TwilioCredentials");
                var twilioDto = twilioCredentials.Get<TwilioDto>();

                TwilioClient.Init(twilioDto.AccountSSD, twilioDto.AuthToken);


                //dictionary of people to send the messages to 
                var people = new Dictionary<string, string>()
            {
                {
                    "+254742267032", "Humphry"
                }
            };
                var messageBody = $"Hello Humphry, welcome to Twilio, this is a message from {request.Name} : {request.Message}";

                foreach (var person in people)
                {
                    MessageResource.Create(
                        from: new PhoneNumber("+12056565415"),
                        to: new PhoneNumber(person.Key),
                        body: messageBody);
                }
                #endregion

                return Unit.Value;

                throw new Exception("Prolem saving Changes");
            }
        }

    }
}