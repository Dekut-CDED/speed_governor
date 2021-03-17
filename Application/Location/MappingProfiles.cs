using AutoMapper;
using location = Domain.Location;

namespace Application
{
    public class MappingProfile: Profile
  {
      public MappingProfile()
      {
          CreateMap<location, LocationDto>();
      }
  }
}
