namespace Services.Catalog.Features.Products;

public sealed record ProductDto(Guid? ProductId,
                                string? Name,
                                string? Size,
                                decimal? Price,
                                decimal? PriceSale,
                                string? Category,
                                string? Brand,
                                string? Supplier);
