using Newtonsoft.Json;

namespace tfl_stats.Server.Models.JourneyModels
{
    public class JourneyResponse
    {
        [JsonProperty("journeys")]
        public List<Journey> Journeys { get; set; } = new List<Journey>();
    }
}
