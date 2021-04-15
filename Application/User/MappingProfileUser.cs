using AutoMapper;
using Domain;

namespace Application.User
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // CreateMap<Activity, ActivityDto>();
            CreateMap<AppUser, User>();
            CreateMap<AppUser, UserCacheDto>();
        }
    }
}
