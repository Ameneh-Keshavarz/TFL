﻿using Microsoft.AspNetCore.Mvc;
using tfl_stats.Server.Models;
using tfl_stats.Server.Services;

namespace tfl_stats.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineController : ControllerBase
    {
        private LineService _lineService;
        private ILogger<LineController> _logger;

        public LineController(LineService lineService, ILogger<LineController> logger)
        {
            _lineService = lineService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetLine()
        {
            var response = await _lineService.GetLine();

            if (!response.IsSuccessful)
            {
                switch (response.ResponseStatus)
                {

                    case ResponseStatus.NotFound:
                        _logger.LogWarning("TfL line request returned NotFound.");
                        return NotFound();

                    default:
                        _logger.LogError("TfL line request failed with unexpected error.");
                        return StatusCode(500);
                }
            }
            _logger.LogInformation("Successfully fetched lines.");
            return Ok(response.Data);
        }
    }
}
