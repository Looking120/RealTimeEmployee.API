using Microsoft.AspNetCore.Identity;
using RealTimeEmployee.DataAccess.Constants;

namespace RealTimeEmployee.DataAccess.SeedData;

public class SeedRoles
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public SeedRoles(RoleManager<IdentityRole<Guid>> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task InitializeRolesAsync()
    {
        string[] roleNames = [Roles.Admin, Roles.User, Roles.Employee];

        foreach (var roleName in roleNames)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (roleExist)
            {
                continue;
            }

            var role = new IdentityRole<Guid>(roleName);
            await _roleManager.CreateAsync(role);
        }
    }
}