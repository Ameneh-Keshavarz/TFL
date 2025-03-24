using Newtonsoft.Json;
using tfl_stats.Server.Models.LineModels;

namespace tfl_stats.Server.Services.LineService
{
    public class LineService : ILineService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public LineService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<Line>> getLine()
        {
            string? appId = _configuration["TflApi:AppId"];
            string? appKey = _configuration["TflApi:AppKey"];
            string? baseUrl = _configuration["TflApi:BaseUrl"];

            if (string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(appKey) || string.IsNullOrEmpty(baseUrl))
            {
                throw new InvalidOperationException("TflApi configuration is missing.");
            }

            string url = $"{baseUrl}Line/Mode/tube/Status?app_id={appId}&app_key={appKey}";

            var response = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<List<Line>>(response) ?? new List<Line>();
        }
    }
}
