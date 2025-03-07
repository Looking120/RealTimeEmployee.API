using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RealTimeEmployee.BusinessLogic.Helpers;
using RealTimeEmployee.BusinessLogic.Profiles;
using RealTimeEmployee.BusinessLogic.Services.Implementations;
using RealTimeEmployee.BusinessLogic.Services.Interfaces;
using RealTimeEmployee.DataAccess.Data;
using RealTimeEmployee.DataAccess.Entitites;
using RealTimeEmployee.DataAccess.SeedData;
using System.Text;

namespace RealTimeEmployee.API.Extensions;

public static partial class ApplicationDependenciesConfiguration
{
    public static IServiceCollection ConfigureServices(this WebApplicationBuilder builder)
    {
        builder
            .AddLogger()
            .AddRepositories()
            .AddServices()
            .AddValidators()
            .AddAutoMapper(typeof(MappingProfile));

        return builder.Services;
    }

    /// <summary>
	/// Configures Cross-Origin Resource Sharing (CORS) for the application
	/// </summary>
	/// <param name="builder">The WebApplicationBuilder used to configure services and middleware</param>
	public static void ConfigureCrossOriginRessourceSharing(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy.WithOrigins("*")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
        });
    }

    /// <summary>
	/// Adds services to the <paramref name="services"/> collection
	/// </summary>
	///<param name="services">The <see cref="IServiceCollection"/> to which services are added</param>
	/// <returns>The service collection</returns>
	public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<IUserService, UserService>()
            .AddScoped<IAuthService, AuthService>();

        return services;
    }

    /// <summary>
    /// Configure the services database
    /// </summary>
    /// <param name="builder">The web application builder</param>
    public static IServiceCollection ConfigureDatabase(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("RealTimeEmployeeConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("The connection string is missing or not configured");
        }

        return builder.Services.AddIdentityDatabase(options =>
        {
            options.UseSqlServer(connectionString);
        });
    }

    /// <summary>
	/// Configures fluent validation
	/// </summary>
	/// <param name="services"></param>
	/// <returns>The service collection with added validators</returns>
	public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        return services.AddValidatorsFromAssembly(BusinessLogic.AssemblyReference.Assembly);
    }

    /// <summary>
	/// Configure swagger for API documentation
	/// </summary>
	/// <param name="builder"></param>
	public static IServiceCollection ConfigureSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please Enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "Jwt",
                Scheme = "bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string>()
                }
            });
        });

        return services;
    }

    /// <summary>
    /// Configure JWT authentication
    /// </summary>
    /// <param name="builder">The WebApplicationBuilder</param>
    /// <exception cref="InvalidOperationException"></exception>
    public static IServiceCollection ConfigureJwtAuthentication(this WebApplicationBuilder builder)
    {
        var jwtSection = builder.Configuration.GetSection("Jwt");
        if (!jwtSection.Exists() || !ValidateJwtSettings(jwtSection))
        {
            throw new InvalidOperationException("JWT configuration values are missing or invalid");
        }

        builder.Services.Configure<JwtSettings>(jwtSection);
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSection["Issuer"],
                    ValidAudience = jwtSection["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!))
                };
            });

        return builder.Services;
    }

    public static IServiceCollection AddIdentityDatabase(this IServiceCollection services, Action<DbContextOptionsBuilder> options)
    {
        services
            .AddDbContext<RealTimeEmployeeDbContext>(options)
            .AddIdentity<AppUser, IdentityRole<Guid>>(optionsIdentity =>
            {
                optionsIdentity.User.RequireUniqueEmail = true;
                optionsIdentity.Password.RequireNonAlphanumeric = true;
                optionsIdentity.Password.RequireLowercase = false;
                optionsIdentity.Password.RequireUppercase = true;
                optionsIdentity.Password.RequireDigit = true;
                optionsIdentity.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                optionsIdentity.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
            })
            .AddEntityFrameworkStores<RealTimeEmployeeDbContext>();

        return services;
    }

    /// <summary>
    /// Extension method to configure serilog
    /// </summary>
    /// <param name="builder">The web application builder</param>
    /// <returns>The service collection</returns>
    

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<SeedRoles>()
            .AddScoped<SeedAdmin>();

        return services;
    }

    /// <summary>
	/// Method to validate jwt
	/// </summary>
	/// <param name="jwtSection"></param>
	/// <returns></returns>
	private static bool ValidateJwtSettings(IConfigurationSection jwtSection)
    {
        return !string.IsNullOrEmpty(jwtSection["Issuer"]) &&
               !string.IsNullOrEmpty(jwtSection["Audience"]) &&
               !string.IsNullOrEmpty(jwtSection["Key"]);
    }
}
