namespace tfl_stats.Server.Models.JourneyModels
{
    public class Path
    {
        public string LineString { get; set; } = string.Empty;
        public List<StopPointMatch> StopPoints { get; set; } = new List<StopPointMatch>();
    }
}
