using Microsoft.AspNetCore.Mvc;
using tfl_stats.Server.Services;

namespace tfl_stats.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecorderController : ControllerBase
    {
        private readonly ResponseRecorderService _recorderService;

        public RecorderController(ResponseRecorderService recorderService)
        {
            _recorderService = recorderService;
        }

        [HttpPost("record/on")]
        public IActionResult EnableRecording()
        {
            _recorderService.EnableRecording();
            return Ok(new { message = "Recording enabled" });
        }

        [HttpPost("record/off")]
        public IActionResult DisableRecording()
        {
            _recorderService.DisableRecording();
            return Ok(new { message = "Recording disabled" });
        }

        [HttpPost("playback/on")]
        public IActionResult EnablePlayback()
        {
            _recorderService.EnablePlayback();
            return Ok(new { message = "Playback enabled" });
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save()
        {
            await _recorderService.SaveAsync();
            return Ok(new { message = "Data saved" });
        }

        [HttpPost("load")]
        public async Task<IActionResult> Load()
        {
            await _recorderService.LoadAsync();
            return Ok(new { message = "Data loaded" });
        }
    }
}
