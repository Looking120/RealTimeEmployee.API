using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Helpers;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.BusinessLogic.Services.Interfaces;
using RealTimeEmployee.DataAccess.Entitites;

namespace RealTimeEmployee.BusinessLogic.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ILogger<AuthService> _logger;
    private readonly IValidator<UserSignInRequest> _signInValidator;
    private readonly IValidator<UserRegisterRequest> _registerValidator;
    private readonly IMapper _mapper;

    public AuthService(IUserService userService,
        UserManager<AppUser> userManager,
        ILogger<AuthService> logger,
        IValidator<UserSignInRequest> signInValidator,
        IValidator<UserRegisterRequest> registerValidator,
        SignInManager<AppUser> signInManager,
        IMapper mapper)
    {
        _userService = userService;
        _userManager = userManager;
        _logger = logger;
        _signInValidator = signInValidator;
        _registerValidator = registerValidator;
        _signInManager = signInManager;
        _mapper = mapper;
    }

    public async Task<TokenDto> SignInAsync(UserSignInRequest request, CancellationToken cancellationToken)
    {
        await _signInValidator.ValidateAndThrowAsync(request, cancellationToken);

        var user = await _userService.GetUserByEmail(request.Email);
        await _userService.CheckPassword(user, request.Password);
        await _userService.EnsureEmailConfirmed(user);

        return await _userService.GenerateTokenAsync(user);
    }

    public async Task<TokenDto> SignUpAsync(UserRegisterRequest request, CancellationToken cancellationToken)
    {
        await _registerValidator.ValidateAndThrowAsync(request, cancellationToken);

        if (await _userManager.FindByEmailAsync(request.Email) is not null)
        {
            _logger.LogError("A user with the email {Email} already exists", request.Email);
            throw new InvalidOperationException($"A user with the email {request.Email} already exists");
        }

        var user = _mapper.Map<AppUser>(request);
        user.EmailConfirmed = true;

        var result = await _userManager.CreateAsync(user, request.Password);
        result.ThrowExceptionIfResultDoNotSucceed(_logger);

        await _userManager.AddToRoleAsync(user, "User");

        return await _userService.GenerateTokenAsync(user);
    }

    public async Task SignOutAsync(Guid userId)
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User with id {UserId} signed out successfully", userId);
    }
}
