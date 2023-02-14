using Microsoft.Extensions.Logging;
using PointOfSale.Shared.Dto;
using PointOfSale.Shared.Interfaces.Application;
using PointOfSale.Shared.Interfaces.DataAccess;

namespace PointOfSale.Application
{
    public class ReportingApplication : IReportingApplication
    {
        private readonly IReadRepo _readRepo;
        private readonly ILogger<ReportingApplication> _logger;
        public ReportingApplication(IReadRepo readRepo, ILogger<ReportingApplication> logger)
        {
            _readRepo = readRepo;
            _logger = logger;
        }

        public async Task<Dictionary<DateTime, int>> GetSalesByDay()
        {
            _logger.LogInformation("Loading sales by day.");
            var result = await _readRepo.GetSalesByDay();
            _logger.LogInformation($"Loaded sale records of '{result.Count}' day's.");

            return result;
        }

        public async Task<Dictionary<DateTime, decimal?>> GetRevenueByDay()
        {
            _logger.LogInformation("Loading revenue by day.");
            var result = await _readRepo.GetRevenueByDay();
            _logger.LogInformation($"Loaded sale revenue records of '{result.Count}' day's.");

            return result;
        }

        public async Task<List<SaleDto>> GetGroupedRevenue()
        {
            _logger.LogInformation("Loading revenue grouped by articles.");
            var result = await _readRepo.GetGroupedRevenue();
            _logger.LogInformation($"Loaded sale revenue records of '{result.Count}' day's.");

            return result;
        }
    }
}