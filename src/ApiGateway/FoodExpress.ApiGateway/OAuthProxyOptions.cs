namespace FoodExpress.ApiGateway;

internal sealed class OAuthProxyOptions
{
    public const string SectionName = "Extensions";

    public string ClientId { get; init; } = string.Empty;
    public string ClientSecret { get; init; } = string.Empty;
    
    public List<string> Scopes { get; init; } = [];
    
    public string NameClaim { get; init; } = "preferred_username";
    public string RoleClaim { get; init; } = "roles";
}
