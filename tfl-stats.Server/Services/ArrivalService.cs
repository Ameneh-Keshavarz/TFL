using tfl_stats.Core.Client.Generated;
using tfl_stats.Server.Models;

namespace tfl_stats.Server.Services
{
    public class ArrivalService
    {
        private readonly LineClient _lineClient;
        private readonly ILogger<ArrivalService> _logger;
        public ArrivalService(LineClient lineClinet, ILogger<ArrivalService> logger)
        {
            _lineClient = lineClinet;
            _logger = logger;
        }
        public async Task<ResponseResult<List<Prediction>>> GetArrival(string lineName, string stationId)
        {
            var response = await _lineClient.ArrivalsAsync([lineName], stationId, null, null);
            if (response != null)
            {
                return new ResponseResult<List<Prediction>>(true, response.ToList(), ResponseStatus.Ok);
            }
            return new ResponseResult<List<Prediction>>(false, new List<Prediction>(), ResponseStatus.NotFound);
        }
    }
}
