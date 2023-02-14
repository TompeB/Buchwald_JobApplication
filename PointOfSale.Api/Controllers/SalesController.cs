using Microsoft.AspNetCore.Mvc;
using PointOfSale.Shared.Dto;
using PointOfSale.Shared.Interfaces.Application;

namespace PointOfSale.Api.Controllers
{
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

        [HttpPost(Name = "Post Sale")]
        public async Task<ActionResult> Sale([FromBody] SaleDto sale)
        {
            await _application.ProcessSale(sale);

            _logger.LogInformation("The sale event as processed successfully.");

            return Created("", sale);
        }
    }
}