using System.Security.Claims;

namespace Opsphere.Helpers;

public static class  ClaimsExtentions
{
    public static string? GetUsername(this ClaimsPrincipal user)
    {
        return user.Claims.SingleOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"))?.Value;
    }
}