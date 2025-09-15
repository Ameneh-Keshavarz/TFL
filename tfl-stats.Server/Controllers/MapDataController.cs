using Microsoft.AspNetCore.Mvc;
using tfl_stats.Server.Services;

namespace tfl_stats.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapDataController : ControllerBase
    {
        private MapDataService _mapDataService;
        private ILogger<MapDataController> _logger;
        public MapDataController(MapDataService mapDataService, ILogger<MapDataController> logger)
        {
            _mapDataService = mapDataService;
            _logger = logger;
        }

        [HttpGet]
        public string GetMapData()
        {
            return _mapDataService.GetMapData();
        }
    }
}
