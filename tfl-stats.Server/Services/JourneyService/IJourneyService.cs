using tfl_stats.Server.Models.JourneyModels;

namespace tfl_stats.Server.Services.JourneyService
{
    public interface IJourneyService
    {
        Task<List<Journey>> getJourney(JourneyRequest journey);
    }
}
