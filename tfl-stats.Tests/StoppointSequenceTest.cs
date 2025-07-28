using System.Text.Json;
using tfl_stats.Core.Client.Generated;
namespace tfl_stats.Tests
{
    public class StoppointSequenceTest
    {
        private readonly LineClient _lineClient;
        public StoppointSequenceTest()
        {
            var baseUrl = "https://api.tfl.gov.uk";
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            _lineClient = new LineClient(httpClient);
        }
        [Fact]
        public async Task TestStoppointSequence()
        {
            var id = "940GZZLUBND";
            var serviceTypes = new List<Anonymous9>();

            var response = (await _lineClient.RouteSequenceAsync("Circle", Direction.Inbound, [Anonymous6.Regular], null))
                                .StopPointSequences.ToList();

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync("line_branch_stoppoints.json", json);
            Assert.True(File.Exists("line_branch_stoppoints.json"));

        }

    }
}
