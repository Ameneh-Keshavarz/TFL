using Newtonsoft.Json;

namespace tfl_stats.Server.Models.JourneyModels
{
    public class Journey
    {
        [JsonProperty("startDateTime")]
        public DateTime StartDateTime { get; set; }

        [JsonProperty("arrivalDateTime")]
        public DateTime ArrivalDateTime { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("alternativeRoute")]
        public bool AlternativeRoute { get; set; }

        [JsonProperty("legs")]
        public List<JourneyLeg> Legs { get; set; }
    }
}
