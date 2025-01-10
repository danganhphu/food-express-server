namespace Services.Catalog.Api.Contracts;

/// <summary>
/// Contains the API endpoint routes.
/// </summary>
internal static class ApiRoutes
{
    /// <summary>
    /// Contains the brand routes.
    /// </summary>
    internal static class Brand
    {
        internal const string List = "/";
        internal const string GetById = "/{id}";
    }
}
