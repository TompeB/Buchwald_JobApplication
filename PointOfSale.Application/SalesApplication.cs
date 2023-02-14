using AutoMapper;
using PointOfSale.Domain.Models;
using PointOfSale.Shared.Dto;
using PointOfSale.Shared.Interfaces.Application;
using PointOfSale.Shared.Interfaces.DataAccess;
using PointOfSale.Shared.Interfaces.Services;

namespace PointOfSale.Application
{
    public class SalesApplication : ISalesApplication
    {
        private readonly IWriteRepo _writeRepo;
        private readonly ITrackingService _trackingApplication;
        private readonly IMapper _mapper;
        public SalesApplication(IWriteRepo writeRepo, ITrackingService trackingApplication, IMapper mapper)
        {
            _writeRepo = writeRepo;
            _trackingApplication = trackingApplication;
            _mapper = mapper;
        }

        public async Task ProcessSale(SaleDto sale)
        {
            var saleBlo = _mapper.Map<SaleBlo>(sale);

            saleBlo.ValidateSale();

            await _writeRepo.SaveSale(sale);

            await _trackingApplication.TrackItems(sale);
        }
    }
}