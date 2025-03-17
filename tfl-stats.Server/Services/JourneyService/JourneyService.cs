using Newtonsoft.Json;
using tfl_stats.Server.Models.JourneyModels;

namespace tfl_stats.Server.Services.JourneyService
{
    public class JourneyService : IJourneyService
    {
        private readonly HttpClient _httpclient;
        private readonly IConfiguration _configuration;

        public JourneyService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpclient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<Journey>> getJourney(JourneyRequest journeyRequest)
        {
            string appId = _configuration["TflApi:AppId"];
            string appKey = _configuration["TflApi:AppKey"];
            string baseUrl = _configuration["TflApi:BaseUrl"];

            //string from = await GetStopPointId("Kingston", appId, appKey, baseUrl);
            //string to = await GetStopPointId("Piccadilly", appId, appKey, baseUrl);

            string from = await GetStopPointId(journeyRequest.From, appId, appKey, baseUrl);
            string to = await GetStopPointId(journeyRequest.To, appId, appKey, baseUrl);

            if (from == null || to == null)
            {
                return new List<Journey>();
            }

            string url = $"{baseUrl}Journey/journeyresults/{Uri.EscapeDataString(from)}/to/{Uri.EscapeDataString(to)}?app_id={appId}&app_key={appKey}";

            var responseContent = await _httpclient.GetStringAsync(url);

            var journeyResponse = JsonConvert.DeserializeObject<JourneyResponse>(responseContent);

            return journeyResponse?.Journeys ?? new List<Journey>();
        }

        private async Task<string> GetStopPointId(string location, string appId, string appKey, string baseUrl)
        {
            string url = $"{baseUrl}StopPoint/Search/{Uri.EscapeDataString(location)}?app_id={appId}&app_key={appKey}";
            var responseContent = await _httpclient.GetStringAsync(url);
            var stopPointResponse = JsonConvert.DeserializeObject<StopPointResponse>(responseContent);

            return stopPointResponse?.Matches?.FirstOrDefault()?.Id;
        }
    }
}
