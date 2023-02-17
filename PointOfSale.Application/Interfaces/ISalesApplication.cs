using PointOfSale.Shared.Dto;

namespace PointOfSale.Application.Interfaces;
public interface ISalesApplication
{
    Task ProcessSale(SaleDto sale);
}