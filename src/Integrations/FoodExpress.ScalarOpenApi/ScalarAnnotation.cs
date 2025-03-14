namespace FoodExpress.ScalarOpenApi;

internal sealed record ScalarAnnotation(
    string[] DocumentNames,
    string Route,
    EndpointReference EndpointReference
) : IResourceAnnotation;
