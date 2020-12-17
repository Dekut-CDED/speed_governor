using AfricasTalkingCS;
using Application.Interfaces;
using Domain;
using System;

namespace Infrastructure.Message
{
    public class MessageService : IMessage
    {

        static AfricasTalkingGateway gateway = new AfricasTalkingGateway(AfricasTalkingConstants.Username, AfricasTalkingConstants.Apikey, AfricasTalkingConstants.Env);

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

    }
}
