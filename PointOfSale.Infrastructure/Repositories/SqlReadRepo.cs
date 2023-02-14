using PointOfSale.Infrastructure.Context;
using PointOfSale.Shared.Dto;
using PointOfSale.Shared.Interfaces.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace PointOfSale.Infrastructure.Repositories
{
    public class SqlReadRepo : IReadRepo
    {
        private readonly SalesContext _dbContext;

        public SqlReadRepo(SalesContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SaleDto>> GetGroupedRevenue()
        {
            return await _dbContext
                                    .Sales
                                    .GroupBy(s => s.ArticleNumber)
                                    .Select(x => new SaleDto { 
                                        ArticleNumber = x.Key, 
                                        SalesPrice = x.Sum(y => y.SalesPrice)
                                    })
                                    .ToListAsync();
        }

        public async Task<Dictionary<DateTime, decimal?>> GetRevenueByDay()
        {
            return (await _dbContext
                                    .Sales
                                    .GroupBy(s => s.TimeStampCreated.Date)
                                    .Select(x => new {
                                        Date = x.Key,
                                        Price = x.Sum(y => y.SalesPrice)
                                    })
                                    .ToListAsync())
                                .ToDictionary(x => x.Date, x => x.Price);
        }

        public async Task<Dictionary<DateTime, int>> GetSalesByDay()
        {
            return (await _dbContext
                                    .Sales
                                    .GroupBy(s => s.TimeStampCreated.Date)
                                    .Select(x => new {
                                        Date = x.Key,
                                        Count = x.Count()
                                    })
                                    .ToListAsync())
                                .ToDictionary(x => x.Date, x => x.Count);
        }
    }
}
