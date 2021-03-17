using AutoMapper;
using location = Domain.Location;

namespace Application
{
    public class MappingProfiles : Profile
  {
      public MappingProfiles()
      {
          CreateMap<location, LocationDto>();
      }
  }
}
