using tfl_stats.Core.Client.Generated;
using tfl_stats.Server.Models.StopPointModels;
using tfl_stats.Server.Services.Cache;

namespace tfl_stats.Server.Services
{
    public class StopPointService
    {
        private readonly StopPointClient _stopPointClinet;
        private readonly ICacheService _cache;
        private readonly ILogger<StopPointService> _logger;

        private static readonly SemaphoreSlim _preloadLock = new(1, 1);

        public StopPointService(
            StopPointClient stopPointClinet,
            ICacheService cache,
            ILogger<StopPointService> logger)
        {
            _stopPointClinet = stopPointClinet;
            _cache = cache;
            _logger = logger;
        }

        public async Task<List<StopPointSummary>> PreloadStopPoints()
        {
            await _preloadLock.WaitAsync();

            try
            {
                var cached = await _cache.GetAsync<List<StopPointSummary>>(CacheKeys.AllStopPoints, FetchAllStopPoints);
                return cached!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error preloading stop points.");
                return new List<StopPointSummary>();
            }
            finally
            {
                _preloadLock.Release();
            }
        }

        public async Task<List<StopPointSummary>> FetchAllStopPoints(string _)
        {
            var modes = new[] { "tube" };
            var response = await _stopPointClinet.GetByModeAsync(modes, null);

            if (response == null)
            {
                _logger.LogWarning("API returned no stop points.");
                return new List<StopPointSummary>();
            }

            var stopPoints = response.StopPoints
                .Where(sp => sp.StopType == "NaptanMetroStation")
                .Select(sp => new StopPointSummary
                {
                    NaptanId = sp.NaptanId,
                    CommonName = sp.CommonName
                })
                .ToList();

            await _cache.SetAsync(CacheKeys.AllStopPoints, stopPoints, TimeSpan.FromDays(1));
            _logger.LogInformation("Stop points fetched from API and cached.");

            return stopPoints;
        }

        public async Task<List<StopPointSummary>> GetAutocompleteSuggestions(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<StopPointSummary>();

            try
            {
                var cachedSuggestions = await _cache.GetAsync<List<StopPointSummary>>(query, GetFilteredStopPoints);
                return cachedSuggestions!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing autocomplete for '{Query}'", query);
                return new List<StopPointSummary>();
            }
        }

        public async Task<List<StopPointSummary>> GetFilteredStopPoints(string query)
        {
            var allStopPoints = await PreloadStopPoints();

            var suggestions = allStopPoints
                .Where(sp => sp.CommonName.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Take(10)
                .ToList();

            if (suggestions.Any())
            {
                var cacheKey = CacheKeys.Autocomplete(query);

                await _cache.SetAsync(cacheKey, suggestions, TimeSpan.FromDays(1));
                _logger.LogInformation("Autocomplete suggestions cached for '{Query}'", query);
            }
            else
            {
                _logger.LogInformation("No autocomplete suggestions found for '{Query}'", query);
            }

            return suggestions;
        }

        internal async Task<List<StopPointSummary>> GetStopPointList()
        {
            return await PreloadStopPoints();
        }
    }
}