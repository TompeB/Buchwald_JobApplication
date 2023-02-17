using Microsoft.AspNetCore.Mvc;
using PointOfSale.Shared.Dto;
using PointOfSale.Application.Interfaces;

namespace PointOfSale.Api.Controller;

/// <summary>
/// Provides endpoints for storing and tracking sale events
/// </summary>
[ApiController]
[Route("[controller]")]
public class SalesController : ControllerBase
{
    private readonly ILogger<SalesController> _logger;
    private readonly ISalesApplication _application;

    public SalesController(ILogger<SalesController> logger, ISalesApplication application)
    {
        _logger = logger;
        _application = application;
    }

    /// <summary>
    /// Process a sale event.
    /// </summary>
    /// <param name="sale">The sale event data</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Sales
    ///     {
    ///        "ArticleNumber": "ANR1234567890",
    ///        "SalesPrice": 30.5
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the processed sale</response>
    /// <response code="400">If the sale couldn't be validated</response>
    [HttpPost(Name = "Post Sale")]
    public async Task<ActionResult> Sale([FromBody] SaleDto sale)
    {
        await _application.ProcessSale(sale);

        _logger.LogInformation("The sale event as processed successfully.");

        return Created("", sale);
    }
}