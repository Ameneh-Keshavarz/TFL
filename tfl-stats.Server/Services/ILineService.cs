using tfl_stats.Server.Models;

namespace tfl_stats.Server.Services
{
    public interface ILineService
    {
        Task<List<Line>> getLine();

    }
}
