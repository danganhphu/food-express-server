namespace BuildingBlocks.SharedKernel.Identity;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal principal)
        => principal.FindFirstValue("sub");

    public static string? GetEmail(this ClaimsPrincipal principal)
        => principal.FindFirstValue(ClaimTypes.Email);

    public static string? GetFullName(this ClaimsPrincipal principal)
        => principal?.FindFirst(FoodExpressClaims.Fullname)?.Value;

    public static string? GetFirstName(this ClaimsPrincipal principal)
        => principal?.FindFirst(ClaimTypes.Name)?.Value;

    public static string? GetSurname(this ClaimsPrincipal principal)
        => principal?.FindFirst(ClaimTypes.Surname)?.Value;

    public static string? GetPhoneNumber(this ClaimsPrincipal principal)
        => principal.FindFirstValue(ClaimTypes.MobilePhone);

    private static string? FindFirstValue(this ClaimsPrincipal principal, string claimType)
        => principal is null
               ? throw new ArgumentNullException(nameof(principal))
               : principal.FindFirst(claimType)?.Value;
}
