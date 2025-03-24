using System.Text.Json.Serialization;

namespace tfl_stats.Server.Models.LineModels
{
    public class Line
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        public string ModeName { get; set; } = string.Empty;

        public List<Disruption> Disruptions { get; set; } = new List<Disruption>();

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        [JsonPropertyName("lineStatuses")]
        public List<LineStatus> LineStatuses { get; set; } = new List<LineStatus>();

        public List<RouteSection> RouteSections { get; set; } = new List<RouteSection>();

        public List<LineServiceTypeInfo> ServiceTypes { get; set; } = new List<LineServiceTypeInfo>();

        public Crowding Crowding { get; set; } = new Crowding();
    }
}
