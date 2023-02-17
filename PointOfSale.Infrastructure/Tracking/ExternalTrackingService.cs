using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PointOfSale.Infrastructure.Config;
using PointOfSale.Shared.Dto;
using PointOfSale.Infrastructure.Interfaces;
using System.Net.Http.Json;

namespace PointOfSale.Application;
public class ExternalTrackingService : ITrackingService
{
    private readonly ExternalServiceSettings _externalServiceSettings;
    private readonly ILogger<ExternalTrackingService> _logger;

    public ExternalTrackingService(IOptions<ExternalServiceSettings> externalServiceSettings, ILogger<ExternalTrackingService> logger) 
    {
        _externalServiceSettings = externalServiceSettings.Value;
        _logger = logger;
    }

    /// <summary>
    /// Tracks items to an external service that than tracks the items and triggers follow up actions, etc.
    /// We can deactivate it with the active flag in the external tracking settings
    /// </summary>
    /// <param name="sale">The sale event that needs to be tracked</param>
    /// <exception cref="TrackingException">When the event couldn't be tracked</exception>
    public async Task TrackItems(SaleDto sale)
    {
        if (!_externalServiceSettings.Active)
            return;

        var client = new HttpClient();

        _logger.LogInformation($"Tracking items at external service.");

        HttpResponseMessage response = await client.PostAsJsonAsync(_externalServiceSettings.ServiceUrl, sale);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();

        _logger.LogInformation($"External seervice tracking was successful. The external tracking service responsed with StatusCode '{response.StatusCode}' and returned the payload '{JsonConvert.SerializeObject(result)}'");
    }
}