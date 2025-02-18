namespace Services.Catalog.Features.Products.Create.v1;

public sealed class CreateProductHandler([FromKeyedServices("catalog")] IEfRepository<Product> repository)
    : ICoreCommandHandler<CreateProductCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = Product.Create(
            command.Name,
            command.Size,
            command.Price,
            command.PriceSale,
            command.CategoryId,
            command.BrandId,
            command.SupplierId);

        var result = await repository.AddAsync(product, cancellationToken);

        return Result.Success(result.Id.Value);
    }
}
