using WeatherAIAgent.Models;

namespace WeatherAIAgent.Services;

/// <summary>
/// Interface for weather data retrieval service
/// </summary>
public interface IWeatherService
{
    /// <summary>
    /// Gets current weather data for the specified city
    /// </summary>
    /// <param name="cityName">Name of the city</param>
    /// <param name="countryCode">Optional country code (e.g., "US", "JP")</param>
    /// <returns>Weather data for the specified location</returns>
    Task<WeatherData?> GetCurrentWeatherAsync(string cityName, string? countryCode = null);

    /// <summary>
    /// Gets weather forecast for the specified city
    /// </summary>
    /// <param name="cityName">Name of the city</param>
    /// <param name="countryCode">Optional country code</param>
    /// <returns>Weather forecast data</returns>
    Task<WeatherData?> GetWeatherForecastAsync(string cityName, string? countryCode = null);
}
