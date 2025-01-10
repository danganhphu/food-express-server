using Ardalis.Specification.EntityFrameworkCore;
using BuildingBlocks.Core.EFCore;

namespace Services.Catalog.Infrastructure.Data;

public sealed class CatalogRepository<T>(CatalogDbContext dbDbContext)
    : RepositoryBase<T>(dbDbContext),
      IEfReadRepository<T>,
      IEfRepository<T>
    where T : class, IAggregateRoot;
