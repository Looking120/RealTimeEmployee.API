using AutoMapper;
using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.Enums;
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

        CreateMap<Office, OfficeDto>()
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Center.Y))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Center.X));

        CreateMap<OfficeCreateRequest, Office>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.Center, opt => opt.Ignore());

        // Employee mapping from AppUser and HireEmployeeRequest to Employee
        CreateMap<(AppUser User, HireEmployeeRequest Request, Office Office), Employee>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
            .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.User.MiddleName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email ?? string.Empty))
            .ForMember(dest => dest.EmployeeNumber, opt => opt.MapFrom(src => src.Request.EmployeeNumber))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Request.PhoneNumber))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Request.Address))
            .ForMember(dest => dest.HireDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src =>
                src.Request.DateOfBirth ?? DateTime.UtcNow.AddYears(-25)))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Request.Gender))
            .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.Request.DepartmentId))
            .ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => src.Request.PositionId))
            .ForMember(dest => dest.CurrentStatus, opt => opt.MapFrom(src => ActivityStatus.Available))
            .ForMember(dest => dest.LastStatusChange, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Office.Center.Y))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Office.Center.X))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Office.Center));
    }
}
