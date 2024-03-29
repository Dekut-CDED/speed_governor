namespace Application.Activities
{
    using AutoMapper;
    using Domain;
    using Domain.Dto;
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserActivity, AttendeeDto>();   //.ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName)).ForMember( d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName));
            CreateMap<SpeedGovernor, SpeedGovernorDto>().ForMember(d => d.Owner, o => o.MapFrom(s => s.Owner.FullName));
            CreateMap<Location, LocationDto>();
            CreateMap<AppUser, NameList>();
            CreateMap<AppUser, User>();
            CreateMap<AppUser, UserCacheDto>();
        }
    }
}
