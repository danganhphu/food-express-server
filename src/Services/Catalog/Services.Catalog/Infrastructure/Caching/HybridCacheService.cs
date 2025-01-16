using Microsoft.Extensions.Caching.Hybrid;

namespace BuildingBlocks.SharedKernel.Caching;

public sealed class HybridCacheService(HybridCache cache) : IHybridCacheService
{
    public async Task<string> GetCachedDataAsync(string key, CancellationToken cancellationToken = default)
        => await cache.GetOrCreateAsync(
               key,
               async token =>
               {
                   await Task.Delay(500, token); // Simulate async operation

                   return "Cached Data for key: " + key;
               },
               cancellationToken: cancellationToken);
}
