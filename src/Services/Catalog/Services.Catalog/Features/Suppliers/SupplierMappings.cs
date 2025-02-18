namespace Services.Catalog.Features.Suppliers;

internal static class SupplierMappings
{
    internal static SupplierDto ToSupplierDto(this Supplier supplier)
        => new(supplier.Id.Value, supplier.Name);

    internal static IReadOnlyCollection<SupplierDto> ToSupplierDtos(this IEnumerable<Supplier> suppliers)
        => suppliers.Select(ToSupplierDto).ToArray();
}
