namespace tfl_stats.Server.Models.LineModels
{
    public class LineStatus
    {
        public int Id { get; set; }
        public string LineId { get; set; } = string.Empty;
        public int StatusSeverity { get; set; }
        public string StatusSeverityDescription { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public List<ValidityPeriod> ValidityPeriods { get; set; } = new List<ValidityPeriod>();
        public Disruption Disruption { get; set; } = new Disruption();
    }
}
