using PointOfSale.Shared.Dto;
using PointOfSale.Shared.Interfaces.Services;

namespace PointOfSale.Application
{
    public class ExternalTrackingService : ITrackingService
    {
        public Task TrackItems(SaleDto sale)
        {
            throw new NotImplementedException();
        }
    }
}
