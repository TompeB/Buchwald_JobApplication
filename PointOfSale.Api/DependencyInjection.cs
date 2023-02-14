using PointOfSale.Application;
using PointOfSale.Infrastructure;
using PointOfSale.Infrastructure.Repositories;
using PointOfSale.Shared.Interfaces.Application;
using PointOfSale.Shared.Interfaces.DataAccess;
using PointOfSale.Shared.Interfaces.Services;

namespace PointOfSale.Api
{
    public static class DependencyInjection
    {
        public static void AddDependencys(this IServiceCollection services)
        {
            services.AddScoped<IReadRepo, SqlReadRepo>();
            services.AddScoped<IWriteRepo, SqlWriteRepo>();
            services.AddSingleton<ITrackingService, EventHubTrackingService>();
            services.AddScoped<ISalesApplication, SalesApplication>();
            services.AddScoped<IReportingApplication, ReportingApplication>();
        }
    }
}
