namespace BuildingBlocks.SharedKernel.Identity;

public sealed class IdentityService(IHttpContextAccessor httpContextAccessor) : IIdentityService
{
    public Guid GetUserId()
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user is null || !IsAuthenticated())
        {
            return Guid.Empty;
        }

        var userIdClaim = user.GetUserId();

        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }

    public string? GetFullName()
        => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);

    public string? GetEmail()
        => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

    public bool IsAdminRole()
        => httpContextAccessor.HttpContext?.User.IsInRole("Admin") ?? false;

    public bool IsAuthenticated()
        => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated is true;
}
