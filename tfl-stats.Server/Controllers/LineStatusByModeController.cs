using Microsoft.AspNetCore.Mvc;
using tfl_stats.Server.Services.LineService;

namespace tfl_stats.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineStatusByModeController : ControllerBase
    {
        private ILineService _lineService;
        public LineStatusByModeController(ILineService lineService)
        {
            _lineService = lineService;
        }

        [HttpGet]
        public async Task<IActionResult> getLine()
        {
            var data = await _lineService.getLine();
            return Ok(data);
        }
    }
}
