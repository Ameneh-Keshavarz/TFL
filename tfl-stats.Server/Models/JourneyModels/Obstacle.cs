using Newtonsoft.Json;

namespace tfl_stats.Server.Models.JourneyModels
{
    public class Obstacle
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("incline")]
        public string Incline { get; set; }

        [JsonProperty("stopId")]
        public long StopId { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }
    }
}
