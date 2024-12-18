using Microsoft.AspNetCore.Authorization;

namespace TravelInspiration.API.Shared.Security;

public static class AuthorizationPolicies
{
    public static AuthorizationPolicy HasWriteActionPolicy =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireClaim("scope", "write")
            .Build();
}
