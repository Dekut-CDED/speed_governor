using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface IMessage
    {
       MessageResponse SendMessage(string message, string phone);
    }
}