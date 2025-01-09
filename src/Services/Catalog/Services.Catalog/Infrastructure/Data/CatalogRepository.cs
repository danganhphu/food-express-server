using Ardalis.Specification.EntityFrameworkCore;
using BuildingBlocks.Core.Domain.Abstractions;
using BuildingBlocks.Core.EFCore;

namespace Services.Catalog.Infrastructure.Data;

public sealed class CatalogRepository<T>(CatalogContext dbContext)
    : RepositoryBase<T>(dbContext),
      IEfReadRepository<T>,
      IEfRepository<T>
    where T : class, IAggregateRoot;
