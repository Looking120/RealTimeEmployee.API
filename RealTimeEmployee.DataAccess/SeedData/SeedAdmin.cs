using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RealTimeEmployee.DataAccess.Constants;
using RealTimeEmployee.DataAccess.Entitites;

namespace RealTimeEmployee.DataAccess.SeedData;

public class SeedAdmin
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SeedAdmin> _logger;

    public SeedAdmin(UserManager<AppUser> userManager, IConfiguration configuration, ILogger<SeedAdmin> logger)
    {
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task InitializesAdminAsync()
    {
        var adminFirstName = _configuration["AdminCredentials:FirstName"];
        var adminLastName = _configuration["AdminCredentials:LastName"];
        var adminMiddleName = _configuration["AdminCredentials:MiddleName"];
        var adminUserName = _configuration["AdminCredentials:UserName"];
        var adminEmail = _configuration["AdminCredentials:Email"];
        var adminPassword = _configuration["AdminCredentials:Password"];

        if (adminFirstName is null || adminLastName is null || adminUserName is null || adminPassword is null || adminEmail == null)
        {
            _logger.LogError("The admin credentials are not properly configured");
            return;
        }

        var adminUser = await _userManager.FindByNameAsync(adminUserName);

        if (adminUser is not null)
            return;

        adminUser = new AppUser { FirstName = adminFirstName, LastName = adminLastName, MiddleName = adminMiddleName, UserName = adminUserName, Email = adminEmail, EmailConfirmed = true };
        var result = await _userManager.CreateAsync(adminUser, adminPassword);

        if (result.Succeeded)
            await _userManager.AddToRoleAsync(adminUser, Roles.Admin);
    }
}