using Newtonsoft.Json;

namespace tfl_stats.Server.Models.JourneyModels
{
    public class Mode
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
    }
}
