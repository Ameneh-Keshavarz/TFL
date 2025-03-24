using Newtonsoft.Json;

namespace tfl_stats.Server.Models.JourneyModels
{
    public class Instruction
    {
        [JsonProperty("summary")]
        public string? Summary { get; set; }

        [JsonProperty("detailed")]
        public string? Detailed { get; set; }
    }
}
