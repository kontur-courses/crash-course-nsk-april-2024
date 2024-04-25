using System.Security.Claims;
using Market.Exceptions;

namespace Market.Misc;

internal static class ClaimsPrincipalExtensions
{
    internal static Guid GetUserIdOrDie(this ClaimsPrincipal principal)
    {
        var rawUserId = principal.Claims.FirstOrDefault(s => s.Type == "user-id")?.Value;

        if (rawUserId == null)
            throw ErrorRegistry.UserNotAuthenticated();

        if (Guid.TryParse(rawUserId, out var userId))
            throw ErrorRegistry.UserNotAuthenticated();
        
        return userId;
    }
}