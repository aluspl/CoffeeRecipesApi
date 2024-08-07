using Wolverine;

namespace Api.Extensions;

public static class HostExtensions
{
    public static IHostBuilder UseWolverine(this IHostBuilder host)
    {
        host.UseWolverine(options =>
        {
            // You want this maybe!
            options.Policies.AutoApplyTransactions();
            options.Discovery.IncludeAssembly(typeof(Program).Assembly);

            options.Policies.UseDurableLocalQueues();

        });

        return host;
    }
}