namespace Application.Interfaces
{
    public interface IMessage
    {
       dynamic SendMessage(string message, string phone);
       dynamic SendTwilioMessage(string sender, string phoneNumber);
    }
}