using System.Security.Claims;

namespace MAApi.Helpers.Identity
{
    public  static class IdentityHelpers
    {
        public static IList<Claim> GetClaimsFromJwt(ClaimsPrincipal httpUser)
        {
            var identity = httpUser.Identity as ClaimsIdentity;
            if (identity == null) throw new UnauthorizedAccessException();
            return identity.Claims.ToList();
        }
    }
}
