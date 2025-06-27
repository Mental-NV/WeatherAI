using Newtonsoft.Json;
using WeatherAIAgent.Models;

namespace WeatherAIAgent.Services;

/// <summary>
/// Service for retrieving weather data from OpenWeatherMap API
/// </summary>
public class OpenWeatherMapService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<OpenWeatherMapService> _logger;
    private readonly string _apiKey;
    private readonly string _baseUrl;

    public OpenWeatherMapService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<OpenWeatherMapService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;

        _apiKey = _configuration["OpenWeatherMap:ApiKey"] ?? 
                  throw new ArgumentException("OpenWeatherMap API key not configured");
        
        _baseUrl = _configuration["OpenWeatherMap:BaseUrl"] ?? 
                   "https://api.openweathermap.org/data/2.5";
    }

    /// <inheritdoc />
    public async Task<WeatherData?> GetCurrentWeatherAsync(string cityName, string? countryCode = null)
    {
        try
        {
            var query = string.IsNullOrEmpty(countryCode) ? cityName : $"{cityName},{countryCode}";
            var url = $"{_baseUrl}/weather?q={Uri.EscapeDataString(query)}&appid={_apiKey}&units=metric";

            _logger.LogInformation("Fetching current weather for: {Query}", query);

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Weather API request failed with status: {StatusCode}", response.StatusCode);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var weatherResponse = JsonConvert.DeserializeObject<OpenWeatherMapResponse>(content);

            if (weatherResponse == null)
            {
                _logger.LogWarning("Failed to deserialize weather response");
                return null;
            }

            return MapToWeatherData(weatherResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching weather data for {CityName}", cityName);
            return null;
        }
    }

    /// <inheritdoc />
    public async Task<WeatherData?> GetWeatherForecastAsync(string cityName, string? countryCode = null)
    {
        try
        {
            var query = string.IsNullOrEmpty(countryCode) ? cityName : $"{cityName},{countryCode}";
            var url = $"{_baseUrl}/forecast?q={Uri.EscapeDataString(query)}&appid={_apiKey}&units=metric&cnt=1";

            _logger.LogInformation("Fetching weather forecast for: {Query}", query);

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Weather forecast API request failed with status: {StatusCode}", response.StatusCode);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            // For forecast, we'll need to parse the forecast response format
            // For now, fall back to current weather
            return await GetCurrentWeatherAsync(cityName, countryCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching weather forecast for {CityName}", cityName);
            return null;
        }
    }

    /// <summary>
    /// Maps OpenWeatherMap response to our internal WeatherData model
    /// </summary>
    private static WeatherData MapToWeatherData(OpenWeatherMapResponse response)
    {
        return new WeatherData
        {
            City = response.Name ?? "Unknown",
            Country = response.Sys?.Country ?? "Unknown",
            Temperature = response.Main?.Temp ?? 0,
            FeelsLike = response.Main?.FeelsLike ?? 0,
            Description = response.Weather?.FirstOrDefault()?.Description ?? "Unknown",
            Humidity = response.Main?.Humidity ?? 0,
            WindSpeed = response.Wind?.Speed ?? 0,
            Pressure = response.Main?.Pressure ?? 0,
            Timestamp = DateTimeOffset.FromUnixTimeSeconds(response.Dt).DateTime
        };
    }
}
