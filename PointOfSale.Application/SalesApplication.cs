using AutoMapper;
using PointOfSale.Domain.Models;
using PointOfSale.Shared.Dto;
using PointOfSale.Application.Interfaces;
using PointOfSale.Infrastructure.Interfaces;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace PointOfSale.Application;
public class SalesApplication : ISalesApplication
{
    private readonly IWriteRepo _writeRepo;
    private readonly ITrackingService _trackingApplication;
    private readonly IMapper _mapper;
    private readonly ILogger<SalesApplication> _logger;
    public SalesApplication(IWriteRepo writeRepo, ITrackingService trackingApplication, IMapper mapper, ILogger<SalesApplication> logger)
    {
        _writeRepo = writeRepo;
        _trackingApplication = trackingApplication;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task ProcessSale(SaleDto sale)
    {
        _logger.LogInformation($"Processing sale event.{JsonConvert.SerializeObject(sale)}");

        var saleBlo = _mapper.Map<SaleBlo>(sale);

        saleBlo.ValidateSale();

        _logger.LogInformation($"The sale event was validated. Saving it to the database...");

        await _writeRepo.SaveSale(sale);

        _logger.LogInformation($"The sale event was saved to the database.");

        await _trackingApplication.TrackItems(sale);
    }
}