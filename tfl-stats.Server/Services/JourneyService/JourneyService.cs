using Newtonsoft.Json;
using tfl_stats.Server.Models.JourneyModels;

namespace tfl_stats.Server.Services.JourneyService
{
    public class JourneyService
    {
        private readonly HttpClient _httpclient;
        private readonly IConfiguration _configuration;
        private readonly string appId;
        private readonly string appKey;
        private readonly string baseUrl;

        public JourneyService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpclient = httpClient;
            _configuration = configuration;
            appId = _configuration["TflApi:AppId"] ?? throw new ArgumentNullException(nameof(appId));
            appKey = _configuration["TflApi:AppKey"] ?? throw new ArgumentNullException(nameof(appKey));
            baseUrl = _configuration["TflApi:BaseUrl"] ?? throw new ArgumentNullException(nameof(baseUrl));
        }

        public async Task<List<Journey>> getJourney(JourneyRequest journeyRequest)
        {
            string? from = await GetStopPointId(journeyRequest.From, appId, appKey, baseUrl);
            string? to = await GetStopPointId(journeyRequest.To, appId, appKey, baseUrl);

            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
            {
                return new List<Journey>();
            }

            string url = $"{baseUrl}Journey/journeyresults/{Uri.EscapeDataString(from)}/to/{Uri.EscapeDataString(to)}?app_id={appId}&app_key={appKey}";

            var responseContent = await _httpclient.GetStringAsync(url);

            var journeyResponse = JsonConvert.DeserializeObject<JourneyResponse>(responseContent);
            return journeyResponse?.Journeys ?? new List<Journey>();
        }

        private async Task<string?> GetStopPointId(string location, string appId, string appKey, string baseUrl)
        {
            string url = $"{baseUrl}StopPoint/Search/{Uri.EscapeDataString(location)}?app_id={appId}&app_key={appKey}";
            var responseContent = await _httpclient.GetStringAsync(url);
            var stopPointResponse = JsonConvert.DeserializeObject<StopPointResponse>(responseContent);

            var bestMatch = stopPointResponse?.Matches?
                .Where(sp => sp.Modes.Contains("tube")).FirstOrDefault();

            return bestMatch?.IcsId ?? string.Empty;
        }

        public async Task<List<string>> GetAutocompleteSuggestions(string query)
        {
            if (string.IsNullOrEmpty(query))
                return new List<string>();

            string url = $"{baseUrl}StopPoint/Search/{Uri.EscapeDataString(query)}?app_id={appId}&app_key={appKey}";

            var response = await _httpclient.GetStringAsync(url);
            var stopPointResponse = JsonConvert.DeserializeObject<StopPointResponse>(response);

            return stopPointResponse?.Matches?.Select(sp => sp.Name).ToList() ?? new List<string>();
        }
    }
}
