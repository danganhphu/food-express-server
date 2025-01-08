using Duende.Bff;

namespace FoodExpress.ApiGateway;

/// <summary>
///     Configuration section
/// </summary>
internal class Configuration
{
    public string? Authority { get; set; }

    public string? ClientId { get; set; }

    /// <summary>
    ///     should be supplied as a command line argument or environment variable, e.g.
    ///     ./GenericBFF --BFF:ClientSecret=secret
    /// </summary>
    public string? ClientSecret { get; set; }

    public List<string> Scopes { get; set; } = [];
    public List<Api> Apis { get; set; } = [];
}

internal class Api
{
    public string? LocalPath { get; set; }
    public string? RemoteUrl { get; set; }
    public TokenType RequiredToken { get; set; }
}
