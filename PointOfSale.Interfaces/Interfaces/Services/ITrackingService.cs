using PointOfSale.Shared.Dto;

namespace PointOfSale.Shared.Interfaces.Services
{
    public interface ITrackingService
    {
        Task TrackItems(SaleDto sale);
    }
}
