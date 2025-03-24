using Newtonsoft.Json;

namespace tfl_stats.Server.Models.JourneyModels
{
    public class StopPointResponse
    {
        [JsonProperty("matches")]
        public List<StopPointMatch> Matches { get; set; } = new List<StopPointMatch>();
    }
}
