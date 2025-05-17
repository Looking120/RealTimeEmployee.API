using AutoMapper;
using FluentValidation;
using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Exceptions;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.BusinessLogic.Services.Interfaces;
using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.Models;
using RealTimeEmployee.DataAccess.Repository.Interfaces;

namespace RealTimeEmployee.BusinessLogic.Services.Implementations;

public class OrganizationService : IOrganizationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<DepartmentCreateRequest> _departmentValidator;
    private readonly IValidator<PositionCreateRequest> _positionValidator;

    public OrganizationService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<DepartmentCreateRequest> departmentValidator,
        IValidator<PositionCreateRequest> positionValidator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _departmentValidator = departmentValidator;
        _positionValidator = positionValidator;
    }

    public async Task<DepartmentDto> CreateDepartmentAsync(DepartmentCreateRequest request)
    {
        await _departmentValidator.ValidateAndThrowAsync(request);

        var departmentRepo = _unitOfWork.GetRepository<Department>();

        // Check if department already exists
        if (await departmentRepo.ExistsAsync(d => d.Name == request.Name))
            throw new AlreadyExistsException($"Department '{request.Name}' already exists");

        var department = _mapper.Map<Department>(request);

        await departmentRepo.AddAsync(department);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<DepartmentDto>(department);
    }

    public async Task<PositionDto> CreatePositionAsync(PositionCreateRequest request)
    {
        await _positionValidator.ValidateAndThrowAsync(request);

        var positionRepo = _unitOfWork.GetRepository<Position>();
        var departmentRepo = _unitOfWork.GetRepository<Department>();

        if (!await departmentRepo.ExistsAsync(d => d.Id == request.DepartmentId))
            throw new NotFoundException($"Department with ID {request.DepartmentId} not found");


        if (await positionRepo.ExistsAsync(p => p.Title == request.Title))
            throw new AlreadyExistsException($"Position '{request.Title}' already exists");

        var position = _mapper.Map<Position>(request);
        await positionRepo.AddAsync(position);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<PositionDto>(position);
    }

    public async Task AssignPositionToDepartmentAsync(Guid positionId, Guid departmentId)
    {
        var positionRepo = _unitOfWork.GetRepository<Position>();
        var departmentRepo = _unitOfWork.GetRepository<Department>();

        var position = await positionRepo.GetByIdAsync(positionId);

        if (position == null)
            throw new NotFoundException($"Position with ID {positionId} not found");

        if (!await departmentRepo.ExistsAsync(d => d.Id == departmentId))
            throw new NotFoundException($"Department with ID {departmentId} not found");

        position.DepartmentId = departmentId;
        positionRepo.Update(position);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
    {
        var departments = await _unitOfWork.GetRepository<Department>().GetAllAsync();

        return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
    }

    public async Task<IEnumerable<PositionDto>> GetAllPositionsAsync()
    {
        var positions = await _unitOfWork.GetRepository<Position>().GetAllAsync();

        return _mapper.Map<IEnumerable<PositionDto>>(positions);
    }

    public async Task<PaginatedResult<DepartmentDto>> GetAllDepartmentsAsync(PaginationRequest pagination)
    {
        var departmentRepo = _unitOfWork.GetRepository<Department>();
        var totalCount = await departmentRepo.CountAsync();

        var departments = await departmentRepo.GetPagedAsync(pagination);
        var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(departments.Items);

        return new PaginatedResult<DepartmentDto>(
            departmentDtos,
            totalCount,
            pagination.PageNumber,
            pagination.PageSize);
    }

    public async Task<PaginatedResult<PositionDto>> GetAllPositionsAsync(PaginationRequest pagination)
    {
        var positionRepo = _unitOfWork.GetRepository<Position>();
        var totalCount = await positionRepo.CountAsync();

        var positions = await positionRepo.GetPagedAsync(pagination);
        var positionDtos = _mapper.Map<IEnumerable<PositionDto>>(positions.Items);

        return new PaginatedResult<PositionDto>(
            positionDtos,
            totalCount,
            pagination.PageNumber,
            pagination.PageSize);
    }
}