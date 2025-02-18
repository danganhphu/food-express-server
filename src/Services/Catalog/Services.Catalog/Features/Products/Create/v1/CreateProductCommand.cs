namespace Services.Catalog.Features.Products.Create.v1;

public sealed record CreateProductCommand(string? Name,
                                          string? Size,
                                          decimal Price,
                                          decimal PriceSale,
                                          CategoryId CategoryId,
                                          BrandId BrandId,
                                          SupplierId SupplierId) : ICoreCommand<Result<Guid>>;
