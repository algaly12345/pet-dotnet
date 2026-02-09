using System.Security.Claims;

namespace pets.Security
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool HasPermission(this ClaimsPrincipal user, string permission)
        {
            return user?.Claims?.Any(c => c.Type == "permission" && c.Value == permission) == true;
        }
    }
}
