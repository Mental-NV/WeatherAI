using Microsoft.AspNetCore.Mvc;
using WeatherAIAgent.Models;
using WeatherAIAgent.Services;

namespace WeatherAIAgent.Controllers;

/// <summary>
/// Controller for handling weather-related AI agent requests
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherAIService _weatherAIService;
    private readonly ILogger<WeatherController> _logger;

    public WeatherController(
        IWeatherAIService weatherAIService,
        ILogger<WeatherController> logger)
    {
        _weatherAIService = weatherAIService;
        _logger = logger;
    }

    /// <summary>
    /// Processes a natural language weather query using AI
    /// </summary>
    /// <param name="request">The weather query request containing the user's natural language input</param>
    /// <returns>AI-generated response with weather information</returns>
    /// <response code="200">Successfully processed the weather query</response>
    /// <response code="400">Invalid request or missing query</response>
    /// <response code="500">Internal server error occurred while processing the query</response>
    [HttpPost("query")]
    [ProducesResponseType(typeof(WeatherQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<WeatherQueryResponse>> QueryWeatherAsync([FromBody] WeatherQueryRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Query))
        {
            _logger.LogWarning("Invalid weather query request received");
            return BadRequest("Query cannot be empty");
        }

        try
        {
            _logger.LogInformation("Received weather query: {Query}", request.Query);

            var response = await _weatherAIService.ProcessWeatherQueryAsync(request.Query);
            
            if (!response.Success && !string.IsNullOrEmpty(response.ErrorMessage))
            {
                _logger.LogError("Weather query processing failed: {ErrorMessage}", response.ErrorMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error processing weather query");
            
            return StatusCode(StatusCodes.Status500InternalServerError, new WeatherQueryResponse
            {
                Response = "An unexpected error occurred while processing your request.",
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    /// <summary>
    /// Health check endpoint for the weather service
    /// </summary>
    /// <returns>Service health status</returns>
    [HttpGet("health")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public ActionResult GetHealth()
    {
        return Ok(new { 
            status = "healthy", 
            timestamp = DateTime.UtcNow,
            service = "Weather AI Agent"
        });
    }
}
