using System.Security.Claims;
using System.Security.Principal;

namespace ShoppingCart.Helpers
{
    public static class ClaimsHelper
    {
        public static long GetUserId(this ClaimsPrincipal claims)
        {
            var userId = ((ClaimsIdentity)claims.Identity).FindFirst("UserId");
            return userId is null ? throw new ArgumentException("UserId is required") : Convert.ToInt64(userId.Value);
        }

        public static short GetRoleId(this ClaimsPrincipal claims)
        {
            var roleId = ((ClaimsIdentity)claims.Identity).FindFirst("Role");
            return roleId is null ? throw new ArgumentException("RoleId is required") : Convert.ToInt16(roleId.Value);
        }

        public static long GetSellerId(this ClaimsPrincipal claims)
        {
            var sellerId = ((ClaimsIdentity)claims.Identity).FindFirst("SellerId");
            return sellerId is null ? throw new ArgumentException("SellerId is required") : Convert.ToInt16(sellerId.Value);
        }

        public static void AddOrUpdateClaim(this IPrincipal currentPrincipal, string key, string value)
        {
            var identity = currentPrincipal.Identity as ClaimsIdentity;
            if (identity == null)
                return;

            var existingClaim = identity.FindFirst(key);
            if (existingClaim != null)
                identity.RemoveClaim(existingClaim);

            identity.AddClaim(new Claim(key, value));
        }
    }
}
