using Microsoft.AspNetCore.Mvc;
using PointOfSale.Api.Responses;
using PointOfSale.Application.Interfaces;

namespace PointOfSale.Api.Controllers;

/// <summary>
/// The reporting controller provides Statictics and Reports of all sale events that occured in the applications lifetime
/// </summary>
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

    /// <summary>
    /// Returns all sales by day across all articles
    /// </summary>
    /// <returns>All sales by day</returns>
    /// <response code="200">All sales by day</response>
    /// <response code="500">Service partly not available or other issues occured</response>
    [HttpGet("~/Reporting/SalesByDay", Name = "SalesByDay")]
    public async Task<ActionResult<SalesByDayResponse>> GetSalesByDay()
    {
        return new SalesByDayResponse(await _reportingApplication.GetSalesByDay());
    }

    /// <summary>
    /// Returns all total revenue by day across all articles
    /// </summary>
    /// <returns>A The total revenue by day</returns>
    /// <response code="200">The total revenue by day</response>
    /// <response code="500">Service partly not available or other issues occured</response>
    [HttpGet("~/Reporting/RevenueByDay", Name = "RevenueByDay")]
    public async Task<ActionResult<RevenueByDayResponse>> GetOverallRevenueByDay()
    {
        return new RevenueByDayResponse(await _reportingApplication.GetRevenueByDay());
    }

    /// <summary>
    /// Returns the total revenue by all articles that were ever sold
    /// </summary>
    /// <returns>The total revenue by all articles</returns>
    /// <response code="200">Returns a list of the total revenue by article number</response>
    /// <response code="500">Service partly not available or other issues occured</response>
    [HttpGet("~/Statistics/RevenueByArticle", Name = "RevenueByArticle")]
    public async Task<ActionResult<RevenueByArticleResponse>> GetRevenue()
    {
        return new RevenueByArticleResponse(await _reportingApplication.GetGroupedRevenue());
    }
}
