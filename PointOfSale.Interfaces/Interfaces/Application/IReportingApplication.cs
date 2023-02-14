using PointOfSale.Shared.Dto;

namespace PointOfSale.Shared.Interfaces.Application
{
    public interface IReportingApplication
    {
        Task<List<SaleDto>> GetGroupedRevenue();

        Task<Dictionary<DateTime, decimal?>> GetRevenueByDay();

        Task<Dictionary<DateTime, int>> GetSalesByDay();
    }
}
