using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PointOfSale.Infrastructure.Config;
using PointOfSale.Shared.Dto;
using PointOfSale.Shared.Interfaces.Services;
using System.Text;

namespace PointOfSale.Application;
public class EventHubTrackingService : ITrackingService
{
    private readonly string _connectionString;
    private readonly string _eventHubName;

    public EventHubTrackingService(IOptions<ConnectionStrings> connectionStrings, IOptions<EventHubSettings> eventHubSettings)
    {
        _eventHubName = eventHubSettings.Value.Name;
        _eventHubName = connectionStrings.Value.EventHub;
    }

    public async Task TrackItems(SaleDto sale)
    {
        return;
        var producerClient = new EventHubProducerClient(_connectionString, _eventHubName);

        using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

        eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(sale))));

        try
        {
            await producerClient.SendAsync(eventBatch);
        }
        finally
        {
            await producerClient.DisposeAsync();
        }
    }
}