using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RealTimeEmployee.DataAccess.Entitites;
using System.Text;

namespace RealTimeEmployee.BusinessLogic.Helpers;

public static class UserHelper
{
    public static void ThrowExceptionIfResultDoNotSucceed(this IdentityResult identityResult, ILogger logger)
    {
        if (identityResult.Succeeded)
        {
            return;
        }

        var cause = identityResult.Errors.Select(x => x.Description)
            .Aggregate(new StringBuilder(), (builder, description) => builder.Append(description));

        logger.LogError("Identity operation failed: {cause}", cause);

        throw new InvalidOperationException(cause.ToString());
    }

    public static async Task<T> AddRoleToUserAsync<T>(this UserManager<T> userManager, string role, T user, ILogger logger) where T : AppUser
    {
        var roleResult = await userManager.AddToRoleAsync(user, role);

        roleResult.ThrowExceptionIfResultDoNotSucceed(logger);

        return user;
    }

    public static async Task<bool> IsUserEmailExist<T>(this UserManager<T> userManager, string email) where T : AppUser
    {
        var userLooked = await userManager.FindByEmailAsync(email);

        return userLooked is not null;
    }
}