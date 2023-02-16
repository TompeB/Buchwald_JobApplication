using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PointOfSale.Api;
using PointOfSale.Infrastructure.Config;
using PointOfSale.Infrastructure.Context;

namespace PointOfSale.IntegrationTests
{
    public static class HostingContext
    {
        public static HttpClient Client { get; private set; }
        public static WebApplicationFactory<Program> Factory;

        public static void StartUpHost()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.integrationtest.json", optional: false)
                .Build();

            Factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder
                    .ConfigureAppConfiguration(y => y
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddConfiguration(configuration))
                    .ConfigureServices(services => {
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType ==
                                typeof(DbContextOptions<SalesContext>));

                        services.Remove(descriptor);

                        services.AddDbContext<SalesContext>(options =>
                        {
                            options.UseInMemoryDatabase("InMemoryDbForTesting");
                        });
                    })
                );

            Client = Factory.CreateClient();
        }

        public static T GetService<T>() where T : class => Factory.Services.CreateScope().ServiceProvider.GetService<T>();
    }
}
