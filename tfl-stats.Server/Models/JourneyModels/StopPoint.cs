using Newtonsoft.Json;

namespace tfl_stats.Server.Models.JourneyModels
{
    public class StopPoint
    {
        [JsonProperty("naptanId")]
        public string NaptanId { get; set; }

        [JsonProperty("commonName")]
        public string CommonName { get; set; }

        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lon")]
        public double Longitude { get; set; }
    }
}
