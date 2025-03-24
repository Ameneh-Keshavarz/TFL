using Microsoft.AspNetCore.Mvc;
using tfl_stats.Server.Services.JourneyService;

namespace tfl_stats.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StopPointController : ControllerBase
    {
        private JourneyService _journeyService;

        public StopPointController(JourneyService journeyService) => this._journeyService = journeyService;


        [HttpGet("autocomplete")]
        public async Task<IActionResult> GetAutocompleteSuggestions([FromQuery] string query)
        {
            var suggestions = await _journeyService.GetAutocompleteSuggestions(query);
            return Ok(suggestions);
        }

    }

}
