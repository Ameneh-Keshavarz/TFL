using System.Text.Json;
using tfl_stats.Core.Client.Generated;

namespace tfl_stats.Tests
{
    public class StopPointMapLineTest
    {
        private readonly StopPointClient _stopPointClient;

        public StopPointMapLineTest()
        {
            var baseUrl = "https://api.tfl.gov.uk";
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };

            _stopPointClient = new StopPointClient(httpClient);
        }

        [Fact]
        public async Task TestStopPointMapLine()
        {
            var modes = new[] { "tube", "dlr", "elizabeth-line" };
            var response = await _stopPointClient.GetByModeAsync(modes, null);

            var results = response?.StopPoints
                ?.Where(sp => sp.StopType == "NaptanMetroStation" || sp.StopType == "NaptanRailStation")
                .ToList();

            var json = JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });

            await File.WriteAllTextAsync("stop_points.json", json);

            Assert.True(File.Exists("stop_points.json"));
            Assert.DoesNotContain(results, sp => sp.Lines == null || sp.Lines.Count == 0);
            Console.WriteLine(results.Count);
        }
    }
}
