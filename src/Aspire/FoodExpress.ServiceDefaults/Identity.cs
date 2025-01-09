namespace FoodExpress.ServiceDefaults;

public sealed class Identity
{
#pragma warning disable CA1056
    public string Url { get; init; } = string.Empty;
#pragma warning restore CA1056
    public string Audience { get; init; } = string.Empty;
}
