using Microsoft.AspNetCore.Mvc;
using tfl_stats.Server.Models;
using tfl_stats.Server.Services;

namespace tfl_stats.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArrivalController : ControllerBase
    {
        private ArrivalService _arrivalService;
        private ILogger<ArrivalController> _logger;

        public ArrivalController(ArrivalService arrivalService, ILogger<ArrivalController> logger)
        {
            _arrivalService = arrivalService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetArrival(string lineName, string stationId)
        {
            var response = await _arrivalService.GetArrival(lineName, stationId);
            if (!response.IsSuccessful)
            {
                switch (response.ResponseStatus)
                {
                    case ResponseStatus.NotFound:
                        _logger.LogWarning("Not Found");
                        return NotFound();

                    default:
                        _logger.LogError("Unexpected Error");
                        return StatusCode(500);
                }


            }
            _logger.LogInformation("Successfully fetched Arrivals.");
            return Ok(response.Data);
        }
    }
}
