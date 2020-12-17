using System;
using AfricasTalkingCS;
using Application.Interfaces;
using Domain;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Message
{
    public class Message : IMessage
    {
        private readonly IConfiguration _config;
        static AfricasTalkingGateway gateway;

        public Message(IConfiguration config)
        {
            _config = config;
        }

        public MessageResponse SendMessage(string message, string phone)
        {

            gateway = new AfricasTalkingGateway(_config["AFUsername"], _config["AFApikey"], _config["AFEnv"]);

            try
            {

            var messageDemo = "Hello Africa, Jambo Kenya. Mko sawa Lakini?";

            string recepientDemo = "+254742267032";

            var sms = gateway.SendMessage(recepientDemo,   messageDemo + message +  DateTime.Now);


            return new MessageResponse
            {
            };

            }
            catch (AfricasTalkingGatewayException ex)
            {
                // TODO
                throw new Exception("Proble with Africas Talking", ex);

            }

        }
    }
}



