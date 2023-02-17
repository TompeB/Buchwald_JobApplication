using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PointOfSale.Infrastructure.Config;
using PointOfSale.Shared.Dto;
using PointOfSale.Shared.Exceptions;
using PointOfSale.Infrastructure.Interfaces;
using System.Text;

namespace PointOfSale.Application;
public class EventHubTrackingService : ITrackingService
{
    private readonly string _connectionString;
    private readonly EventHubSettings _eventHubSettings;
    private readonly ILogger<EventHubTrackingService> _logger;

    public EventHubTrackingService(IOptions<ConnectionStrings> connectionStrings, IOptions<EventHubSettings> eventHubSettings, ILogger<EventHubTrackingService> logger)
    {
        _eventHubSettings = eventHubSettings.Value;
        _connectionString = connectionStrings.Value.EventHub;
        _logger = logger;
    }

    /// <summary>
    /// Tracks items to an EventHub consumer that than tracks the items and triggers follow up actions, etc.
    /// We can deactivate it with the active flag in the eventhub settings
    /// </summary>
    /// <param name="sale">The sale event that needs to be tracked</param>
    /// <exception cref="TrackingException">When the event couldn't be published</exception>
    public async Task TrackItems(SaleDto sale)
    {
        if (!_eventHubSettings.Active)
            return;

        _logger.LogInformation("Tracking items by sending an event to eventhub.");

        var producerClient = new EventHubProducerClient(_connectionString, _eventHubSettings.Name);

        using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

        eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(sale))));

        try
        {
            await producerClient.SendAsync(eventBatch);
        }
        catch (Exception ex)
        {
            _logger.LogError("Tracking failed. Something went wrong while send the event to eventub.", ex);
            throw new TrackingException(ex);
        }
        finally
        {
            await producerClient.DisposeAsync();
            _logger.LogInformation($"EventHub tracking was successful.");
        }
    }
}