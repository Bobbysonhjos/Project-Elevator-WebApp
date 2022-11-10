using System.Security.Claims;

namespace WebApp.Helpers.Authorization
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetName(this ClaimsPrincipal? user)
        {
            if (user == null) return "";

            var first = string.Empty;
            var last = string.Empty;

            try
            {
                first = user.Claims.FirstOrDefault(x => x.Type == "given_name")?.Value;
                last = user.Claims.FirstOrDefault(x => x.Type == "family_name")?.Value;
            }
            catch
            {
                // ignored
            }

            return first + " " + last;
        }
    }
}
