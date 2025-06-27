namespace WeatherAIAgent.Models;

/// <summary>
/// Represents a weather query request from the user
/// </summary>
public class WeatherQueryRequest
{
    /// <summary>
    /// The natural language query from the user
    /// Example: "What's the weather in Tokyo tomorrow?"
    /// </summary>
    public string Query { get; set; } = string.Empty;
}

/// <summary>
/// Represents the AI agent's response to a weather query
/// </summary>
public class WeatherQueryResponse
{
    /// <summary>
    /// The natural language response from the AI agent
    /// </summary>
    public string Response { get; set; } = string.Empty;

    /// <summary>
    /// Structured weather data if available
    /// </summary>
    public WeatherData? WeatherData { get; set; }

    /// <summary>
    /// Indicates if the query was successfully processed
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Error message if any occurred during processing
    /// </summary>
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// Structured weather information
/// </summary>
public class WeatherData
{
    /// <summary>
    /// The city name
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// The country code
    /// </summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// Current temperature in Celsius
    /// </summary>
    public double Temperature { get; set; }

    /// <summary>
    /// "Feels like" temperature in Celsius
    /// </summary>
    public double FeelsLike { get; set; }

    /// <summary>
    /// Weather description (e.g., "clear sky", "light rain")
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Humidity percentage
    /// </summary>
    public int Humidity { get; set; }

    /// <summary>
    /// Wind speed in meters per second
    /// </summary>
    public double WindSpeed { get; set; }

    /// <summary>
    /// Atmospheric pressure in hPa
    /// </summary>
    public int Pressure { get; set; }

    /// <summary>
    /// Timestamp of the weather data
    /// </summary>
    public DateTime Timestamp { get; set; }
}
