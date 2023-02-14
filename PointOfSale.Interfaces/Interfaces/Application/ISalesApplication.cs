using PointOfSale.Shared.Dto;

namespace PointOfSale.Shared.Interfaces.Application
{
    public interface ISalesApplication
    {
        Task ProcessSale(SaleDto sale);
    }
}
