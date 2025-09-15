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
        private readonly List<MapData> _elements = [];

        public void AddStopPoint(string label, string id, double lat, double lon, IEnumerable<string> lines)
        {
            var station = new StationData()
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
            var sequence = new LineData()
            {
                Type = "LineData",
                LineName = line,
                LinePoints = points.Select((t) =>
                {
                    return new PointData() { Lat = t.Item1, Lon = t.Item2 };
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

    [JsonDerivedType(typeof(StationData), "StationData")]
    [JsonDerivedType(typeof(LineData), "LineData")]
    public abstract class MapData
    {
        public required string Type { get; set; }

    }

    public class StationData : MapData
    {
        public required string StationId { get; set; }
        public required string Name { get; set; }
        public string? Url { get; set; }
        public required double Lat { get; set; }
        public required double Lon { get; set; }
        public required List<string> Lines { get; set; }
    }

    public class LineData : MapData
    {
        public string LineName { get; set; }
        public List<PointData> LinePoints { get; set; }
    }

    public class PointData
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }

}