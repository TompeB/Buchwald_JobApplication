using Microsoft.AspNetCore.Mvc;
using PointOfSale.Infrastructure.Responses;
using PointOfSale.Shared.Interfaces.Application;

namespace PointOfSale.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportingController : ControllerBase
    {
        private readonly IReportingApplication _reportingApplication;
        private readonly ILogger<ReportingController> _logger;

        public ReportingController(ILogger<ReportingController> logger, IReportingApplication reportingApplication)
        {
            _logger = logger;
            _reportingApplication = reportingApplication;
        }

        [HttpGet("~/Reporting/SalesByDay", Name = "SalesByDay")]
        public async Task<ActionResult<SalesByDayResponse>> GetSalesByDay()
        {
            return new SalesByDayResponse(await _reportingApplication.GetSalesByDay());
        }

        [HttpGet("~/Reporting/RevenueByDay", Name = "RevenueByDay")]
        public async Task<ActionResult<RevenueByDayResponse>> GetOverallRevenueByDay()
        {
            return new RevenueByDayResponse(await _reportingApplication.GetRevenueByDay());
        }
        
        [HttpGet("~/Statistics/RevenueByArticle", Name = "RevenueByArticle")]
        public async Task<ActionResult<RevenueByArticleResponse>> GetRevenue()
        {
            return new RevenueByArticleResponse(await _reportingApplication.GetGroupedRevenue());
        }
    }
}