using PointOfSale.Shared.Dto;

namespace PointOfSale.Shared.Interfaces.DataAccess
{
    public interface IReadRepo
    {
        public Task<Dictionary<DateTime, int>> GetSalesByDay();
        public Task<Dictionary<DateTime, decimal?>> GetRevenueByDay();
        public Task<List<SaleDto>> GetGroupedRevenue();
    }
}

                                                        