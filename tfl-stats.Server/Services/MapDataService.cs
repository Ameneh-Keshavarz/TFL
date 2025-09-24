using System.Text.Json;
using System.Text.Json.Serialization;
using TflNetworkBuilder;
namespace tfl_stats.Server.Services
{
    public class MapDataService
    {
        string[] _lines = {
            "bakerloo",
            "central",
            "circle",
            "district",
            "hammersmith-city",
            "jubilee",
            "metropolitan",
            "northern",
            "piccadilly",
            "victoria",
            "waterloo-city",
            "dlr",
            "elizabeth"
        };

        internal string GetMapData()
        {
            MapBuilder builder = new MapBuilder(_lines.ToList());
            var map = new JsonMapBuilder();
            builder.BuildMap(map);
            return map.ToString();
        }
    }
    class JsonMapBuilder : IMapClient, IDisposable
    {
        private readonly List<MapElement> _elements = [];

        public void AddStopPoint(string label, string id, double lat, double lon, IEnumerable<string> lines)
        {
            var station = new StationNode()
            {
                Type = "StationData",
                StationId = id,
                Name = label,
                // We need to provide a value for Url
                Url = "",
                Lat = lat,
                Lon = lon,
                Lines = lines.ToList(),
            };
            _elements.Add(station);
        }

        public void AddSequence(string line, IEnumerable<(double, double)> points)
        {
            var sequence = new LinePath()
            {
                Type = "LineData",
                LineName = line,
                LinePoints = points.Select((t) =>
                {
                    return new GeoPoint() { Lat = t.Item1, Lon = t.Item2 };
                }).ToList(),

            };
            _elements.Add(sequence);
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
        }
    }

    [JsonDerivedType(typeof(StationNode), "StationData")]
    [JsonDerivedType(typeof(LinePath), "LineData")]
    public abstract class MapElement
    {
        public required string Type { get; set; }

    }

    public class StationNode : MapElement
    {
        public required string StationId { get; set; }
        public required string Name { get; set; }
        public string? Url { get; set; }
        public required double Lat { get; set; }
        public required double Lon { get; set; }
        public required List<string> Lines { get; set; }
    }

    public class LinePath : MapElement
    {
        public string LineName { get; set; }
        public List<GeoPoint> LinePoints { get; set; }
    }

    public class GeoPoint
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }

}