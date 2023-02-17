using PointOfSale.Infrastructure.Context;
using PointOfSale.Shared.Dto;
using PointOfSale.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PointOfSale.Infrastructure.Repositories;
public class SqlReadRepo : IReadRepo
{
    private readonly SalesContext _dbContext;

    public SqlReadRepo(SalesContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Groups the sale events by articleNumber and calculates the total revenue of this article
    /// </summary>
    /// <returns>A list of all articles that were sold with the total revenue of each article</returns>
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

    /// <summary>
    /// Groups the sale events by the different days they occured at and calculates the total revenue of this day
    /// </summary>
    /// <returns>A list of daily revenues</returns>
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

    /// <summary>
    /// Groups the sale events by the different days they occured at and calculates the total count of sales
    /// </summary>
    /// <returns>A list of sales by day</returns>
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