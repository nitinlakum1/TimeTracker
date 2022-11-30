using System.Security.Claims;

namespace TimeTracker.Helper
{
    public static class SecurityExtensions
    {
        public static Guid GetSubFromClaim(this ClaimsPrincipal principal)
        {
            var claim = principal.FindFirst(x => x.Type == "sub");

            if (claim == null)
            {
                var bb = principal.FindFirst(x => x.Type == ClaimTypes.Name);
                var cc = principal.FindFirst(x => x.Type == ClaimTypes.Email);
                var ff = principal.FindFirst(x => x.Type == ClaimTypes.Role);
                var gg = principal.FindFirst(x => x.Type == ClaimTypes.Gender);
                var hh = principal.FindFirst(x => x.Type == "Id");
            }

            if (claim == null)
            {
                throw new ArgumentException("No Sub claim found");
            }

            return Guid.Parse(claim.Value);
        }

        public static int GetLoginRole(this ClaimsPrincipal principal)
        {
            if (principal.Identity.IsAuthenticated)
            {
                var claim = principal.FindFirst(x => x.Type == "RoleId");
                if (claim == null)
                {
                    throw new ArgumentException("No Role claim found");
                }
                return Convert.ToInt32(claim.Value);
            }
            else
            {
                return 0;
            }
        }

        public static int GetIdFromClaim(this ClaimsPrincipal principal)
        {
            if (principal.Identity.IsAuthenticated)
            {
                var claim = principal.FindFirst(x => x.Type == "Id");
                if (claim == null)
                {
                    throw new ArgumentException("No Id claim found");
                }
                return Convert.ToInt32(claim.Value);
            }
            else
            {
                return 0;
            }
        }
    }
}
