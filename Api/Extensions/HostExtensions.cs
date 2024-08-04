using Marten;
using Wolverine;
using Wolverine.Marten;

namespace Api.Extensions;

public static class HostExtensions
{
    public static IHostBuilder UseWolverine(this IHostBuilder host, string connectionString)
    {
        host.UseWolverine(options =>
        {
            options.Services.AddMarten(connectionString)

                // This adds quite a bit of middleware for
                // Marten
                .IntegrateWithWolverine();

            // You want this maybe!
            options.Policies.AutoApplyTransactions();

            // But wait! Optimize Wolverine for usage as *only*
            // a mediator
            options.Durability.Mode = DurabilityMode.MediatorOnly;
            options.Discovery.IncludeAssembly(typeof(Program).Assembly);

            options.Policies.UseDurableLocalQueues();

        });

        return host;
    }
}