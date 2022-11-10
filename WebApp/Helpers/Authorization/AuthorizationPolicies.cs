using Microsoft.AspNetCore.Authorization;

namespace WebApp.Helpers.Authorization
{
    public class AuthorizationPolicies
    {
        public static readonly string StandardAccess = "standardAccess";
        public static AuthorizationPolicy GetStandardAccessAuthorizationPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Roles.Admin, Roles.SecondlineTechnician).Build();
        }

        public static readonly string AdminAccess = "adminAccess";
        public static AuthorizationPolicy GetAdminAccessAuthorizationPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Roles.Admin).Build();
        }
    }
}
