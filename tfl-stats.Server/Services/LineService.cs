//using tfl_stats.Server.Models.LineModels;
using tfl_stats.Core.Client.Generated;
using tfl_stats.Server.Models;

namespace tfl_stats.Server.Services
{
    public class LineService
    {
        private readonly LineClient _lineClient;

        private ILogger<LineService> _logger;

        public LineService(LineClient lineClient, ILogger<LineService> logger)
        {
            _lineClient = lineClient;
            _logger = logger;
        }

        public async Task<ResponseResult<List<Line>>> GetLine()
        {
            try
            {
                var lineResponse = await _lineClient.StatusByModeAsync(new[] { "tube" }, true, null);

                if (lineResponse != null)
                {
                    return new ResponseResult<List<Line>>(true, lineResponse.ToList(), ResponseStatus.Ok);
                }

                return new ResponseResult<List<Line>>(false, new List<Line>(), ResponseStatus.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching lines");
                return new ResponseResult<List<Line>>(false, new List<Line>(), ResponseStatus.InternalServerError);
            }
        }

    }
}
