using Microsoft.AspNetCore.Mvc;
using tfl_stats.Server.Services;
namespace tfl_stats.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StopPointController : ControllerBase
    {
        private readonly StopPointService _stopPointService;
        private readonly ILogger<StopPointController> _logger;

        public StopPointController(StopPointService stopPointService, ILogger<StopPointController> logger)
        {
            _stopPointService = stopPointService;
            _logger = logger;
        }

        [HttpGet("autocomplete")]
        public async Task<IActionResult> GetAutocompleteSuggestions([FromQuery] string query)
        {
            var data = await _stopPointService.GetAutocompleteSuggestions(query);

            if (data.Count == 0)
            {
                _logger.LogWarning("No autocomplete suggestions found for query: {Query}", query);
            }
            else
            {
                _logger.LogInformation("Fetched {Count} autocomplete suggestions for query: {Query}", data.Count, query);
            }

            return Ok(data);
        }

        [HttpGet("stopPointList")]
        public async Task<IActionResult> GetStopPointList()
        {
            var data = await _stopPointService.GetStopPointList();

            if (data.Count == 0)
            {
                _logger.LogWarning("No stop points returned.");
            }
            else
            {
                _logger.LogInformation("Fetched {Count} stop points.", data.Count);
            }

            return Ok(data);
        }

    }
}