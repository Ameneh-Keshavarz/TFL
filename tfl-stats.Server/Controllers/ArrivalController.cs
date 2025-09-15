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
        public async Task<IActionResult> GetArrival([FromQuery] string[] lines, [FromQuery] string stationId)
        {
            var response = await _arrivalService.GetArrival(lines, stationId);
            if (!response.IsSuccessful)
            {
                switch (response.ResponseStatus)
                {
                    case ResponseStatus.NotFound:
                        _logger.LogWarning("TfL arrival request returned NotFound.");
                        return NotFound();

                    default:
                        _logger.LogError("TfL arrival request failed with unexpected error.");
                        return StatusCode(500);
                }


            }
            _logger.LogInformation("Successfully fetched arrivals.");
            return Ok(response.Data);
        }
    }
}
