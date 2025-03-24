using Newtonsoft.Json;

namespace tfl_stats.Server.Models.JourneyModels
{
    public class StopPointMatch
    {
        [JsonProperty("icsId")]
        public string IcsId { get; set; } = string.Empty;

        [JsonProperty("modes")]
        public string[] Modes { get; set; } = Array.Empty<string>();

        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
    }
}