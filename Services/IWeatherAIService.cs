using WeatherAIAgent.Models;

namespace WeatherAIAgent.Services;

/// <summary>
/// Interface for the AI agent service that processes natural language weather queries
/// </summary>
public interface IWeatherAIService
{
    /// <summary>
    /// Processes a natural language weather query and returns a response
    /// </summary>
    /// <param name="query">The user's natural language query</param>
    /// <returns>AI-generated response with weather information</returns>
    Task<WeatherQueryResponse> ProcessWeatherQueryAsync(string query);
}
