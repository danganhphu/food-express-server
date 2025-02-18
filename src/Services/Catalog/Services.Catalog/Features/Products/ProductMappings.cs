namespace Services.Catalog.Features.Products;

internal static class ProductMappings
{
    internal static ProductDto ToProductDto(this Product product)
        => new(
            product.Id.Value,
            product.Name.Value,
            product.Size.Value,
            product.Price.OriginalPrice,
            product.Price.DiscountPrice,
            product.Category.Name,
            product.Brand.Name,
            product.Supplier.Name);

    internal static IReadOnlyCollection<ProductDto> ToProductDtos(this IEnumerable<Product> products)
        => products.Select(ToProductDto).ToArray();
}
