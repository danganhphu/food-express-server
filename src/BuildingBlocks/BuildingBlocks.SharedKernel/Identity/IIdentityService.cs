namespace BuildingBlocks.SharedKernel.Identity;

public interface IIdentityService
{
    Guid GetUserId();

    string? GetFullName();

    string? GetEmail();

    bool IsAdminRole();
    bool IsAuthenticated();
}
