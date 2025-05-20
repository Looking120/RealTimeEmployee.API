using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealTimeEmployee.BusinessLogic.Dtos;
using RealTimeEmployee.BusinessLogic.Exceptions;
using RealTimeEmployee.BusinessLogic.Helpers;
using RealTimeEmployee.BusinessLogic.Requests;
using RealTimeEmployee.BusinessLogic.Services.Interfaces;
using RealTimeEmployee.DataAccess.Entitites;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealTimeEmployee.BusinessLogic.Services.Implementations;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<UserService> _logger;
    private readonly IValidator<UserSignInRequest> _userValidator;

    public UserService(UserManager<AppUser> userManager,
        IOptions<JwtSettings> jwtSettings, ILogger<UserService> logger,
        IValidator<UserSignInRequest> userValidator)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
        _userValidator = userValidator;
    }

    public async Task CheckPassword(AppUser user, string password)
    {
        if (!await _userManager.CheckPasswordAsync(user, password))
        {
            _logger.LogError("Invalid password or email");
            throw new InvalidCredentialsException("Wrong email or Password, please try again or sign up");
        }
    }

    public async Task EnsureEmailConfirmed(AppUser user)
    {
        if (!await _userManager.IsEmailConfirmedAsync(user))
        {
            _logger.LogError("The user email {Email} is not confirmed", user.Email);
            throw new UnauthorizedException($"The email {user.Email} of the user is not confirmed");
        }
    }

    public async Task<AppUser> GetUserByEmail(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            _logger.LogError("There is no user with that email {Email}", email);
            throw new NotFoundException("This user does not exist");
        }

        return user;
    }

    public async Task ValidateRequest<T>(T request, CancellationToken cancellationToken)
        where T : class
    {
        if (request is UserSignInRequest userSignInRequest)
        {
            var validationResult = await _userValidator.ValidateAsync(userSignInRequest, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogError("Validation failed : {ValidationErrors}", validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }
        }
    }

    public async Task<TokenDto> GenerateTokenAsync(AppUser user)
    {
        if (user.Email == null || user.UserName == null)
        {
            _logger.LogError("User email or userName are null for user with Id {UserId}", user.Id);
            throw new InvalidOperationException("The user email or userName can't be null");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        var userRole = await _userManager.GetRolesAsync(user);
        var role = userRole.First();
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(
            [
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, role),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName)
            ]),

            Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_jwtSettings.DurationInMinutes)),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = _jwtSettings.Audience,
            Issuer = _jwtSettings.Issuer,
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new TokenDto()
        {
            Id = user.Id,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = role,
            AccessToken = tokenHandler.WriteToken(token),
            DurationInMinutes = _jwtSettings.DurationInMinutes
        };
    }
}