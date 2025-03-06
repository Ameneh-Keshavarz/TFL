using Newtonsoft.Json;
using tfl_stats.Server.Models;

namespace tfl_stats.Server.Services
{
    public class LineService : ILineService
    {
        private HttpClient _httpClient;
        private IConfiguration _configuration;

        public LineService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<Line>> getLine()
        {
            string appId = _configuration["TflApi:AppId"];
            string appKey = _configuration["TflApi:AppKey"];
            string baseUrl = _configuration["TflApi:BaseUrl"];


            string url = $"{baseUrl}Line/Mode/tube/Status?app_id={appId}&app_key={appKey}";

            var response = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<List<Line>>(response);
        }
    }
}
