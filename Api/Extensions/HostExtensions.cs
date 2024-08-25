using JasperFx.CodeGeneration;
using JasperFx.CodeGeneration.Commands;
using Wolverine;

namespace Api.Extensions;

public static class HostExtensions
{
    public static IHostBuilder UseWolverine(this IHostBuilder host, bool isProduction)
    {
        host.UseWolverine(options =>
        {
            // You want this maybe!
            options.Policies.AutoApplyTransactions();
            options.Policies.UseDurableLocalQueues();
            // Turn off all logging of the message execution starting and finishing
            // The default is Debug
            options.Policies.MessageExecutionLogLevel(LogLevel.None);

            if (isProduction)
            {
                options.CodeGeneration.TypeLoadMode = TypeLoadMode.Static;

                // You probably only ever want to do this in Production
                options.Services.AssertAllExpectedPreBuiltTypesExistOnStartUp();
            }
        });

        return host;
    }
}