using Microsoft.Extensions.Diagnostics.Enrichment;

namespace BuildingBlocks.SharedKernel.Logging;

public sealed class ApplicationEnricher(IHttpContextAccessor httpContextAccessor) : ILogEnricher
{
    public void Enrich(IEnrichmentTagCollector collector)
    {
        ArgumentNullException.ThrowIfNull(collector);

        collector.Add(LoggingConstant.MachineName, Environment.MachineName);

        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext is not null)
        {
            collector.Add(
                LoggingConstant.UserId,
                httpContext.User.Claims.First(c => c.Type == "sub").Value);
        }
    }
}
