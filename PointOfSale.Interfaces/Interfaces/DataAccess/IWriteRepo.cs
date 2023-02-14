using PointOfSale.Shared.Dto;

namespace PointOfSale.Shared.Interfaces.DataAccess
{
    public interface IWriteRepo
    {
        Task SaveSale(SaleDto sale);
    }
}
