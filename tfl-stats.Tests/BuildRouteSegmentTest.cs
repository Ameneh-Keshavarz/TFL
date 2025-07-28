using System.Text.Json;
using tfl_stats.Core.Client.Generated;

namespace tfl_stats.Tests
{
    public class BuildRouteSegmentTest
    {
        private readonly LineClient _lineClient;

        public BuildRouteSegmentTest()
        {
            var baseUrl = "https://api.tfl.gov.uk";
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            _lineClient = new LineClient(httpClient);
        }

        [Fact]
        public async Task TestRouteSegment()
        {
            //var id = "940GZZLUBND";
            var serviceTypes = new List<Anonymous9>();

            var routeSequence = (await _lineClient.RouteSequenceAsync("central",
                                                                  Direction.Inbound,
                                                                  [Anonymous6.Regular],
                                                                  null));

            var routeSegments = BuildRouteSegments(routeSequence);

            var json = JsonSerializer.Serialize(routeSegments, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync("route-segments.json", json);

            Assert.True(File.Exists("route-segments.json"));
            //Assert.Equal(5, routeSegments.Count);

        }

        Dictionary<int, RouteSegment> BuildRouteSegments(RouteSequence routeSequence)
        {
            var segments = new Dictionary<int, RouteSegment>();
            foreach (var segment in routeSequence.StopPointSequences)
            {
                if (segment.BranchId.HasValue)
                {
                    segments[segment.BranchId.Value] = new RouteSegment
                    {
                        BranchId = segment.BranchId.Value,
                        StopPoints = segment.StopPoint.Select(sp => new MatchedStop
                        {
                            Name = sp.Name,
                            Id = sp.Id,
                            ParentId = sp.ParentId
                        }).ToList(),
                        NextBranchIds = segment.NextBranchIds.ToList()
                    };
                }
            }
            return segments;
        }
    }

    public class RouteSegment
    {
        public int BranchId { get; set; }
        public List<MatchedStop> StopPoints { get; set; } = new();
        public List<int> NextBranchIds { get; set; } = new();
    }
}
