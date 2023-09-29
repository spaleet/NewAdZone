using System.Security.Claims;

namespace BuildingBlocks.Security.Utils;

public static class ClaimsExtension
{
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        if (principal == null)
            throw new ArgumentNullException(nameof(principal));

        var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
        return claim != null ? claim.Value.ToString() : null;
    }
}