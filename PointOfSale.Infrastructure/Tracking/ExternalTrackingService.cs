using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PointOfSale.Infrastructure.Config;
using PointOfSale.Shared.Dto;
using PointOfSale.Shared.Interfaces.Services;
using System.Net.Http.Json;

namespace PointOfSale.Application
{
    public class ExternalTrackingService : ITrackingService
    {
        private readonly ExternalServiceSettings _externalServiceSettings;
        private readonly ILogger<ExternalTrackingService> _logger;

        public ExternalTrackingService(IOptions<ExternalServiceSettings> externalServiceSettings, ILogger<ExternalTrackingService> logger) 
        {
            _externalServiceSettings = externalServiceSettings.Value;
            _logger = logger;
        }

        public async Task TrackItems(SaleDto sale)
        {
            if (!_externalServiceSettings.Active)
                return;

            var client = new HttpClient();

            _logger.LogInformation($"Tracking items at external service.");

            HttpResponseMessage response = await client.PostAsJsonAsync(_externalServiceSettings.ServiceUrl, sale);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();

            _logger.LogInformation($"Tracking was successful. The external tracking service responsed with StatusCode '{response.StatusCode}' and returned the payload '{JsonConvert.SerializeObject(result)}'");
        }
    }
}
