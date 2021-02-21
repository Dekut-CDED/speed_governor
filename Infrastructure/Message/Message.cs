using AfricasTalkingCS;
using Application.Interfaces;
using Domain;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Infrastructure.Message
{
    public class MessageService : IMessage
    {

        static AfricasTalkingGateway gateway = new AfricasTalkingGateway(AfricasTalkingConstants.Username, AfricasTalkingConstants.Apikey, AfricasTalkingConstants.Env);
        private static IConfiguration _configuration;

        public MessageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public  dynamic SendMessage(string recepient, string message)
        {
            
            try
            {
                return gateway.SendMessage(recepient, message + DateTime.Now);
            }
            catch (AfricasTalkingGatewayException exception)
            {
                throw exception;
            }

        }
        public dynamic SendTwilioMessage(string sender, string message)
        {
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
            var messageBody = $"Hello Humphry, welcome to Twilio, this is a message from {sender} : {message}";

            foreach (var person in people)
            {
                MessageResource.Create(
                    from: new PhoneNumber("+12056565415"),
                    to: new PhoneNumber(person.Key),
                    body: messageBody);
            }
            return messageBody;
            #endregion


        }
    }
}
