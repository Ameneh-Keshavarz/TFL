namespace tfl_stats.Server.Models.LineModels
{
    public class LineStatus
    {
        public int Id { get; set; }
        public string LineId { get; set; }
        public int StatusSeverity { get; set; }
        public string StatusSeverityDescription { get; set; }
        public string Reason { get; set; }
        public DateTime Created { get; set; }
        public List<ValidityPeriod> ValidityPeriods { get; set; }
        public Disruption Disruption { get; set; }
    }
}
