using Marten;

namespace Api.App.Infrastructure.Database.Utils;

public class DatabaseInitializer(IDocumentStore store) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var session = store.LightweightSession();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }
}
