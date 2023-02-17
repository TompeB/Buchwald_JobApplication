using PointOfSale.Application;
using PointOfSale.Infrastructure.Repositories;
using PointOfSale.Application.Interfaces;
using PointOfSale.Infrastructure.Interfaces;

namespace PointOfSale.Api;
public static class DependencyInjection
{
    public static void AddDependencys(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IReadRepo, SqlReadRepo>();
        services.AddScoped<IWriteRepo, SqlWriteRepo>();

        if(config.GetValue<bool>("ExternalServiceSettings:Active") == true)
        {
            services.AddSingleton<ITrackingService, ExternalTrackingService>();
        }
        else
        {
            services.AddSingleton<ITrackingService, EventHubTrackingService>();
        }

        services.AddScoped<ISalesApplication, SalesApplication>();
        services.AddScoped<IReportingApplication, ReportingApplication>();
    }
}