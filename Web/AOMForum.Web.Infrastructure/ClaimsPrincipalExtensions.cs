using System.Security.Claims;
using static AOMForum.Common.GlobalConstants;

namespace AOMForum.Web.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        public static string Id(this ClaimsPrincipal user) => user.FindFirst(ClaimTypes.NameIdentifier).Value;

        public static bool IsAdministrator(this ClaimsPrincipal user) => user.IsInRole(AdministratorRoleName);
    }
}