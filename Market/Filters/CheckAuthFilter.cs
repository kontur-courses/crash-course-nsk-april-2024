using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Market.Exceptions;
using Market.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Market.Filters;

public class CheckAuthFilter : ActionFilterAttribute
{
    private readonly UserAuthenticator _userAuthenticator;

    public CheckAuthFilter()
    {
        _userAuthenticator = new UserAuthenticator();
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!AuthenticationHeaderValue.TryParse(context.HttpContext.Request.Headers.Authorization, out var authHeader))
        {
            throw ErrorRegistry.UserNotAuthenticated();
        }
        if (authHeader.Scheme != "Basic")
        {
            throw ErrorRegistry.UserNotAuthenticated();
        }

        if (string.IsNullOrWhiteSpace(authHeader.Parameter))
        {
            throw ErrorRegistry.UserNotAuthenticated();
        }

        if (!TryGetLoginAndPasswordFromHeader(authHeader.Parameter, out var login, out var password))
        {
            throw ErrorRegistry.UserNotAuthenticated();
        }

        var userId = await _userAuthenticator.AuthenticateUser(login, password);
        if (!userId.HasValue)
        {
            throw ErrorRegistry.UserNotAuthenticated();
        }

        AppendUserIdToClaims(userId.Value, context);
        await next();
    }

    private bool TryGetLoginAndPasswordFromHeader(string authHeaderParameter, out string login, out string password)
    {
        login = null!;
        password = null!;

        var credentialsBytes = Convert.FromBase64String(authHeaderParameter);
        var rawCredentials = Encoding.UTF8.GetString(credentialsBytes);
        var credentials = rawCredentials.Split(':');

        if (credentials.Length != 2)
            return false;

        login = credentials[0];
        password = credentials[1];
        return true;
    }

    private void AppendUserIdToClaims(Guid userId, ActionExecutingContext context)
    {
        var claimIdentity = new ClaimsIdentity();
        claimIdentity.AddClaim(new Claim("user-id", userId.ToString()));
        context.HttpContext.User.AddIdentity(claimIdentity);
    }
}