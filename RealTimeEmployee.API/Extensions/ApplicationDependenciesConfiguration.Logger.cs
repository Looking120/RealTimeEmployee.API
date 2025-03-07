using Serilog;

namespace RealTimeEmployee.API.Extensions;

public static partial class ApplicationDependenciesConfiguration
{
    public static IServiceCollection AddLogger(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName();
        });

        return builder.Services;
    }
}
