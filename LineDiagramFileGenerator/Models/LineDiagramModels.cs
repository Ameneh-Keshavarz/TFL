using System.Text.Json.Serialization;

namespace LineDiagramFileGenerator.LineDiagramModels
{
    [JsonDerivedType(typeof(StationName), "stationName")]
    [JsonDerivedType(typeof(Marker), "marker")]
    [JsonDerivedType(typeof(TrackSection), "trackSection")]
    public abstract class LineDiagram
    {
        public required string Type { get; set; }
        public required int Row { get; set; }
        public required int Col { get; set; }
    }

    public class StationName : LineDiagram
    {
        public required string StationId { get; set; }
        public required string Name { get; set; }
        public string? Url { get; set; }
    }

    public class Marker : LineDiagram
    {

    }

    public class TrackSection : LineDiagram
    {
        public required int ColEnd { get; set; }
    }
}
