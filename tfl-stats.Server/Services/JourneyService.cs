﻿using Microsoft.Extensions.Options;
using tfl_stats.Server.Client;
using tfl_stats.Server.Configurations;
using tfl_stats.Server.Models;
using tfl_stats.Server.Models.JourneyModels;


namespace tfl_stats.Server.Services
{
    public class JourneyService
    {
        private readonly ApiClient _apiclient;
        private readonly ILogger<JourneyService> _logger;
        private readonly StopPointService _stopPointService;
        private readonly string appId;
        private readonly string appKey;
        private readonly string baseUrl;

        public JourneyService(ApiClient apiClient,
            IOptions<AppSettings> options,
            StopPointService stopPointService,
            ILogger<JourneyService> logger)
        {
            _apiclient = apiClient;
            _stopPointService = stopPointService;
            _logger = logger;
            appId = options.Value.appId ?? throw new ArgumentNullException(nameof(appId));
            appKey = options.Value.appKey ?? throw new ArgumentNullException(nameof(appKey));
            baseUrl = options.Value.baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
        }

        public async Task<ResponseResult<List<Journey>>> GetJourney(JourneyRequest journeyRequest)
        {
            var from = await _stopPointService.GetStopPointId(journeyRequest.From);
            var to = await _stopPointService.GetStopPointId(journeyRequest.To);

            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
            {
                return new ResponseResult<List<Journey>>(false, new List<Journey>(), ResponseStatus.BadRequest);
            }

            string url = $"{baseUrl}Journey/journeyresults/{Uri.EscapeDataString(from)}/to/{Uri.EscapeDataString(to)}?app_id={appId}&app_key={appKey}";
            var journeyResponse = await _apiclient.GetFromApi<JourneyResponse>(url, "GetJourney");

            if (journeyResponse?.Journeys != null)
            {
                return new ResponseResult<List<Journey>>(true, journeyResponse.Journeys, ResponseStatus.Ok);
            }

            return new ResponseResult<List<Journey>>(false, new List<Journey>(), ResponseStatus.NotFound);
        }
    }

}
