using Newtonsoft.Json;

namespace tfl_stats.Server.Models.JourneyModels
{
    public class JourneyLeg
    {
        [JsonProperty("departureTime")]
        public DateTime DepartureTime { get; set; }

        [JsonProperty("arrivalTime")]
        public DateTime ArrivalTime { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("departurePoint")]
        public StopPoint DeparturePoint { get; set; }

        [JsonProperty("arrivalPoint")]
        public StopPoint ArrivalPoint { get; set; }

        [JsonProperty("instruction")]
        public Instruction Instruction { get; set; }

        [JsonProperty("mode")]
        public Mode Mode { get; set; }

        [JsonProperty("obstacles")]
        public List<Obstacle> Obstacles { get; set; }
    }
}
