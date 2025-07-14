using tfl_stats.Core.Client.Generated;
using tfl_stats.Server.Models;
using tfl_stats.Server.Models.JourneyModels;


namespace tfl_stats.Server.Services
{
    public class JourneyService
    {
        private readonly JourneyClient _journeyclient;
        private readonly ILogger<JourneyService> _logger;
        private readonly StopPointService _stopPointService;

        public JourneyService(JourneyClient journeyclient,
            StopPointService stopPointService,
            ILogger<JourneyService> logger)
        {
            _journeyclient = journeyclient;
            _stopPointService = stopPointService;
            _logger = logger;
        }

        public async Task<ResponseResult<List<Journey2>>> GetJourney(JourneyRequest journeyRequest)
        {


            if (string.IsNullOrEmpty(journeyRequest.FromNaptanId) || string.IsNullOrEmpty(journeyRequest.ToNaptanId))
            {
                return new ResponseResult<List<Journey2>>(false, new List<Journey2>(), ResponseStatus.BadRequest);
            }

            //string url = $"Journey/journeyresults/{Uri.EscapeDataString(journeyRequest.FromNaptanId)}/to/{Uri.EscapeDataString(journeyRequest.ToNaptanId)}";

            try
            {
                var itineraryResult = await _journeyclient.JourneyResultsAsync(
                    from: journeyRequest.FromNaptanId,
                    to: journeyRequest.ToNaptanId,
                    via: null,
                    nationalSearch: false,
                    date: null,
                    time: null,
                    timeIs: null,
                    journeyPreference: null,
                    mode: null,
                    accessibilityPreference: null,
                    fromName: null,
                    toName: null,
                    viaName: null,
                    maxTransferMinutes: null,
                    maxWalkingMinutes: null,
                    walkingSpeed: null,
                    cyclePreference: null,
                    adjustment: null,
                    bikeProficiency: null,
                    alternativeCycle: null,
                    alternativeWalking: null,
                    applyHtmlMarkup: null,
                    useMultiModalCall: null,
                    walkingOptimization: null,
                    taxiOnlyTrip: null,
                    routeBetweenEntrances: null,
                    useRealTimeLiveArrivals: null,
                    calcOneDirection: null,
                    includeAlternativeRoutes: null,
                    overrideMultiModalScenario: null,
                    combineTransferLegs: null
                );

                if (itineraryResult?.Journeys != null)
                {
                    return new ResponseResult<List<Journey2>>(true, itineraryResult.Journeys.ToList<Journey2>(), ResponseStatus.Ok);
                }

                return new ResponseResult<List<Journey2>>(false, new List<Journey2>(), ResponseStatus.NotFound);
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error calling TfL API");
                return new ResponseResult<List<Journey2>>(false, new List<Journey2>(), ResponseStatus.NotFound);
            }
        }
    }
}
