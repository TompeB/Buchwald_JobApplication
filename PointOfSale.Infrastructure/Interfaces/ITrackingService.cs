using PointOfSale.Shared.Dto;

namespace PointOfSale.Infrastructure.Interfaces;
public interface ITrackingService
{
    Task TrackItems(SaleDto sale);
}