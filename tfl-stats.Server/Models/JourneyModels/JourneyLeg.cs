using Newtonsoft.Json;

namespace tfl_stats.Server.Models.JourneyModels
{
    public class JourneyLeg
    {
        [JsonProperty("departureTime")]
        public DateTime DepartureTime { get; set; } = DateTime.MinValue;

        [JsonProperty("arrivalTime")]
        public DateTime ArrivalTime { get; set; } = DateTime.MinValue;

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("departurePoint")]
        public StopPoint DeparturePoint { get; set; } = new StopPoint();

        [JsonProperty("arrivalPoint")]
        public StopPoint ArrivalPoint { get; set; } = new StopPoint();

        [JsonProperty("instruction")]
        public Instruction Instruction { get; set; } = new Instruction();

        [JsonProperty("path")]
        [JsonRequired]
        public Path Path { get; set; } = new Path();

        [JsonProperty("mode")]
        public Mode Mode { get; set; } = new Mode();

        [JsonProperty("obstacles")]
        public List<Obstacle> Obstacles { get; set; } = new List<Obstacle>();
    }
}
