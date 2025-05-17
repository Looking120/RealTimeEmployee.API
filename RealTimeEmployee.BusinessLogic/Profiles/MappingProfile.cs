using AutoMapper;
using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.Models;


namespace RealTimeEmployee.BusinessLogic.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserRegisterRequest, AppUser>()
            .ReverseMap();

        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
            .ForMember(dest => dest.PositionTitle, opt => opt.MapFrom(src => src.Position.Title));

        CreateMap<Department, DepartmentDto>();
        CreateMap<DepartmentCreateRequest, Department>();

        CreateMap<Position, PositionDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));
        CreateMap<PositionCreateRequest, Position>();

        CreateMap<LocationHistory, EmployeeLocationDto>()
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src =>
                $"{src.Employee.FirstName} {src.Employee.LastName}"));

        CreateMap<LocationHistory, EmployeeLocationDto>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src =>
                $"{src.Employee.FirstName} {src.Employee.LastName}"));

        CreateMap<AppUser, UserDto>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore());

        CreateMap(typeof(PaginatedResult<>), typeof(PaginatedResult<>));

        CreateMap<ActivityLog, ActivityLogDto>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src =>
                $"{src.Employee.FirstName} {src.Employee.LastName}"));

        CreateMap<Message, MessageDto>()
            .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src =>
                $"{src.Sender.FirstName} {src.Sender.LastName}"))
            .ForMember(dest => dest.ReceiverName, opt => opt.MapFrom(src =>
                $"{src.Receiver.FirstName} {src.Receiver.LastName}"));
    }
}
