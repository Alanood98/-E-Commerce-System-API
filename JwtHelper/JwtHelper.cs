using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace E_CommerceSystem.Utils
{
    public static class JwtHelper
    {
        public static string? GetUserRoleFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);
                // Extract the role claim
                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                return roleClaim?.Value;
            }
            throw new UnauthorizedAccessException("Invalid or unreadable token.");
        }
    }
}
