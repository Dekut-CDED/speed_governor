using Application.Interfaces;
using location = Domain.Location;

namespace Application
{
    public interface ILocation : IRepository<location>
    {
    }
}