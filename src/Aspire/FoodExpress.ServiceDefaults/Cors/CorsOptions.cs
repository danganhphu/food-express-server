using System.Collections.ObjectModel;

namespace FoodExpress.ServiceDefaults.Cors;

public class CorsOptions
{
    public Collection<string> AllowedOrigins { get; } = [];
}
