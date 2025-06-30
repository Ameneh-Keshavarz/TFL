using Microsoft.Extensions.Logging;
using Moq;
using tfl_stats.Core.Client.Generated;
using tfl_stats.Server.Services;

namespace tfl_stats.Tests
{
    public class LineServiceTest
    {
        private readonly Mock<ILogger<LineService>> _mockLogger;
        private readonly Mock<LineClient> _mockLineClient;
        private readonly LineService _lineService;

        public LineServiceTest()
        {
            _mockLogger = new Mock<ILogger<LineService>>();
            _mockLineClient = new Mock<LineClient>(MockBehavior.Strict, new HttpClient());

            _lineService = new LineService(_mockLineClient.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task TestLineData()
        {
            var testLines = new List<Line>
            {
                new Line
                {
                    Id = "jubilee",
                    Name = "Jubilee",
                    ModeName = "tube",
                    LineStatuses = new List<LineStatus>
                    {
                        new LineStatus
                        {
                            Id = 0,
                            StatusSeverity = 9,
                            StatusSeverityDescription = "Minor Delays",
                            Reason = "Jubilee Line: Minor delays due to train cancellations"
                        }
                    },
                    ServiceTypes = new List<LineServiceTypeInfo>
                    {
                        new LineServiceTypeInfo
                        {
                            Name = "Regular",
                            Uri = "/Line/Route?ids=Jubilee&serviceTypes=Regular"
                        },
                        new LineServiceTypeInfo
                        {
                            Name = "Night",
                            Uri = "/Line/Route?ids=Jubilee&serviceTypes=Night"
                        }
                    },
                    Crowding = new Crowding()
                }
            };

            _mockLineClient
                .Setup(client => client.StatusByModeAsync(It.IsAny<IEnumerable<string>>(), true, null))
                .ReturnsAsync(testLines);

            var result = await _lineService.GetLine();

            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Single(result.Data);
            Assert.Equal("Jubilee", result.Data[0].Name);
            Assert.Equal("Minor Delays", result.Data[0].LineStatuses.First().StatusSeverityDescription);
        }

        [Fact]
        public async Task TestEmptyLineList()
        {
            _mockLineClient
                .Setup(client => client.StatusByModeAsync(It.IsAny<IEnumerable<string>>(), true, null))
                .ReturnsAsync(new List<Line>());

            var result = await _lineService.GetLine();

            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task TestNullLineList()
        {
            List<Line> nullLine = null;

            _mockLineClient
                .Setup(client => client.StatusByModeAsync(It.IsAny<IEnumerable<string>>(), true, null))
                .ReturnsAsync(nullLine);

            var result = await _lineService.GetLine();

            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
        }
    }
}
