using System.Text.Json.Serialization;

namespace tfl_stats.Server.Models
{
    public class Line
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        public string ModeName { get; set; }

        public List<object> Disruptions { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        [JsonPropertyName("lineStatuses")]
        public List<LineStatus> LineStatuses { get; set; }

        public List<RouteSection> RouteSections { get; set; }

        public List<LineServiceTypeInfo> ServiceTypes { get; set; }

        public Crowding Crowding { get; set; }
    }
}
