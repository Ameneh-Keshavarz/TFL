using System.Text.Json;
using tfl_stats.Core.Client.Generated;

namespace tfl_stats.Tests
{
    public class OrderedRouteTest
    {
        private readonly LineClient _lineClient;
        public OrderedRouteTest()
        {
            var baseUrl = "https://api.tfl.gov.uk";
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            _lineClient = new LineClient(httpClient);
        }

        [Fact]
        public async Task TestOrderedRoute()
        {
            var id = "940GZZLUBND";
            var serviceTypes = new List<Anonymous9>();
            var routeSequence = (await _lineClient.RouteSequenceAsync("piccadilly",
                                                                      Direction.Inbound,
                                                                      [Anonymous6.Regular],
                                                                      null));
            var orderedRoute = routeSequence.OrderedLineRoutes;
            var json = JsonSerializer.Serialize(orderedRoute, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync("ordered-route.json", json);
            Assert.True(File.Exists("ordered-route.json"));
            Assert.NotEmpty(orderedRoute);
        }


    }

    public partial class OrderedRoute
    {
        [Newtonsoft.Json.JsonProperty("name", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonProperty("naptanIds", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<string> NaptanIds { get; set; }

        [Newtonsoft.Json.JsonProperty("serviceType", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ServiceType { get; set; }

    }

}
