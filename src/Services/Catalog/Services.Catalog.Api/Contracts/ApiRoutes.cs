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
        internal const string Create = "/";
        internal const string List = "/";
        internal const string GetById = "/{id}";
        internal const string Delete = "/{id}";
    }

    /// <summary>
    /// Contains the category routes.
    /// </summary>
    internal static class Category
    {
        internal const string Create = "/";
        internal const string List = "/";
        internal const string GetById = "/{id}";
        internal const string Delete = "/{id}";
    }

    /// <summary>
    /// Contains the supplier routes.
    /// </summary>
    internal static class Supplier
    {
        internal const string Create = "/";
        internal const string List = "/";
        internal const string GetById = "/{id}";
        internal const string Delete = "/{id}";
    }
}
