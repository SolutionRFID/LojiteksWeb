using System.Security.Claims;

namespace LojiteksWeb.Extensions
{
    public static class LoggedInUserExtensions
    {
        public static string GetLoggedInUsername(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static long GetFirmaID(this ClaimsPrincipal principal)
        {
            string firmaID = principal.FindFirstValue("FirmaID");
            return Convert.ToInt64(firmaID);
        }
    }
}
