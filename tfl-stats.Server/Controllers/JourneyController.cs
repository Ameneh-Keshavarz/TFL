using Microsoft.AspNetCore.Mvc;
using tfl_stats.Server.Models.JourneyModels;
using tfl_stats.Server.Services.JourneyService;

namespace tfl_stats.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JourneyController : ControllerBase
    {
        private IJourneyService _journeyService;
        private ILogger<JourneyController> _logger;

        public JourneyController(IJourneyService journeyService, ILogger<JourneyController> logger)
        {
            this._journeyService = journeyService;
            _logger = logger;

        }

        //[HttpGet]
        //public async Task<IActionResult> getJourney()
        //{
        //    var data = await _journeyService.getJourney();
        //    _logger.LogInformation(data.Count == 0 ? "No data fetched" : "Data fetched");

        //    return Ok(data);

        //}
        [HttpPost]
        public async Task<IActionResult> getJourney([FromBody] JourneyRequest journeyRequest)
        {
            var data = await _journeyService.getJourney(journeyRequest);
            _logger.LogInformation(data.Count == 0 ? "No data fetched" : "Data fetched");

            return Ok(data);

        }
    }
}
