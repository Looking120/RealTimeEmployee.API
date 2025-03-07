using AutoMapper;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.DataAccess.Entitites;


namespace RealTimeEmployee.BusinessLogic.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserRegisterRequest, AppUser>()
            .ReverseMap();
    }
}
