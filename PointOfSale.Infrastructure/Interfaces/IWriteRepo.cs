using PointOfSale.Shared.Dto;

namespace PointOfSale.Infrastructure.Interfaces;

public interface IWriteRepo
{
    Task SaveSale(SaleDto sale);
}