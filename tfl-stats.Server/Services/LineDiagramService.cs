using System.Text.Json;
using System.Text.Json.Serialization;
using TflNetworkBuilder;
namespace tfl_stats.Server.Services
{
    public class LineDiagramService
    {
        public string GetLineDiagram(string lineName)
        {

            var geometry = new GeometryAnalyser(lineName);
            var fileWriterJson = new JsonBuilder(lineName);
            geometry.BuildLineDiagram(fileWriterJson);
            return fileWriterJson.ToString();
        }
    }
    class JsonBuilder : ILineDiagramClient, IDisposable
    {
        private readonly string _line;
        private readonly List<LineDiagram> _elements = [];

        public JsonBuilder(string line)
        {
            _line = line;
        }

        public void AddStationName(int rowNo, int colNo, Station station)
        {
            _elements.Add(new StationName
            {
                Type = "stationName",
                Row = rowNo,
                Col = colNo,
                Name = station?.MatchedStop.FirstOrDefault()?.Name ?? string.Empty,
                StationId = station?.MatchedStop.FirstOrDefault()?.Id ?? string.Empty,
                Url = $"/stations/{station?.StationId}"
            });
        }

        public void AddStopMarker(int rowNo, int colNo)
        {
            _elements.Add(new Marker
            {
                Type = "marker",
                Row = rowNo,
                Col = colNo
            });
        }

        public void AddTrackSection(int rowNo, int colNo, int targetColNo)
        {
            _elements.Add(new TrackSection
            {
                Type = "trackSection",
                Row = rowNo,
                Col = colNo,
                ColEnd = targetColNo
            });
        }

        public string ToString()
        {
            var json = JsonSerializer.Serialize(
                _elements,
                new JsonSerializerOptions { WriteIndented = true });

            return json;

        }

        public void Dispose()
        {
            // nothing to dispose anymore
        }
    }

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