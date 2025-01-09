namespace FoodExpress.ServiceDefaults.OpenApi;

public sealed class OpenApi
{
    public Document Document { get; init; } = new();
    public Auth Auth { get; init; } = new();
}

public sealed class Document
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public sealed class Auth
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string AppName { get; set; } = string.Empty;
}
