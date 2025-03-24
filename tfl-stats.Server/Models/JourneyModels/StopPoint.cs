using Newtonsoft.Json;

namespace tfl_stats.Server.Models.JourneyModels
{
    public class StopPoint
    {
        [JsonProperty("naptanId")]
        public string NaptanId { get; set; } = string.Empty;

        [JsonProperty("commonName")]
        public string CommonName { get; set; } = string.Empty;

        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lon")]
        public double Longitude { get; set; }
    }
}
