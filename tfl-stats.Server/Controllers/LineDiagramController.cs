using Microsoft.AspNetCore.Mvc;
using tfl_stats.Server.Services;

namespace tfl_stats.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineDiagramController : ControllerBase
    {
        private LineDiagramService _lineDiagramService;
        private ILogger<LineDiagramController> _logger;
        public LineDiagramController(LineDiagramService lineDiagramService, ILogger<LineDiagramController> logger)
        {
            _lineDiagramService = lineDiagramService;
            _logger = logger;
        }

        [HttpGet]
        public string GetLineDiagram(string lineName)
        {
            return _lineDiagramService.GetLineDiagram(lineName);
        }
    }
}
