using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.ComponentModel;
using WeatherAIAgent.Models;

namespace WeatherAIAgent.Services;

/// <summary>
/// AI service that uses Semantic Kernel to process weather queries
/// </summary>
public class WeatherAIService : IWeatherAIService
{
    private readonly Kernel _kernel;
    private readonly IWeatherService _weatherService;
    private readonly ILogger<WeatherAIService> _logger;

    public WeatherAIService(
        Kernel kernel,
        IWeatherService weatherService,
        ILogger<WeatherAIService> logger)
    {
        _kernel = kernel;
        _weatherService = weatherService;
        _logger = logger;

        // Register the weather plugin with the kernel
        _kernel.Plugins.AddFromObject(new WeatherPlugin(_weatherService), "Weather");
    }

    /// <inheritdoc />
    public async Task<WeatherQueryResponse> ProcessWeatherQueryAsync(string query)
    {
        try
        {
            _logger.LogInformation("Processing weather query: {Query}", query);

            // Create the system prompt for the AI agent
            var systemPrompt = @"
You are a helpful weather assistant. You can help users get current weather information for any city.
When a user asks about weather, extract the city name and use the available weather functions to get current weather data.
Then provide a natural, conversational response that includes the weather information.

If the user asks about weather forecasts or future weather, explain that you currently only have access to current weather data.
Always be helpful and provide the most relevant information available.

Available functions:
- GetCurrentWeather: Gets current weather for a specified city
";

            // Create execution settings for the AI model
            var executionSettings = new OpenAIPromptExecutionSettings
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
                Temperature = 0.7,
                MaxTokens = 500
            };

            // Process the user query with the AI agent
            var response = await _kernel.InvokePromptAsync(
                $"{systemPrompt}\n\nUser: {query}\nAssistant:",
                new KernelArguments(executionSettings));

            var aiResponse = response.GetValue<string>() ?? "I'm sorry, I couldn't process your weather query.";

            _logger.LogInformation("AI response generated successfully");

            return new WeatherQueryResponse
            {
                Response = aiResponse,
                Success = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing weather query: {Query}", query);

            return new WeatherQueryResponse
            {
                Response = "I'm sorry, I encountered an error while processing your weather query. Please try again.",
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }
}

/// <summary>
/// Semantic Kernel plugin for weather-related functions
/// </summary>
public class WeatherPlugin
{
    private readonly IWeatherService _weatherService;

    public WeatherPlugin(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    /// <summary>
    /// Gets current weather for a specified city
    /// </summary>
    /// <param name="cityName">The name of the city</param>
    /// <param name="countryCode">Optional country code</param>
    /// <returns>Current weather information</returns>
    [KernelFunction("GetCurrentWeather")]
    [Description("Gets the current weather for a specified city")]
    public async Task<string> GetCurrentWeatherAsync(
        [Description("The name of the city")] string cityName,
        [Description("Optional country code (e.g., US, JP, GB)")] string? countryCode = null)
    {
        var weatherData = await _weatherService.GetCurrentWeatherAsync(cityName, countryCode);

        if (weatherData == null)
        {
            return $"I couldn't find weather information for {cityName}. Please check the city name and try again.";
        }

        return $"The current weather in {weatherData.City}, {weatherData.Country} is {weatherData.Description} " +
               $"with a temperature of {weatherData.Temperature:F1}°C (feels like {weatherData.FeelsLike:F1}°C). " +
               $"Humidity is {weatherData.Humidity}%, wind speed is {weatherData.WindSpeed:F1} m/s, " +
               $"and atmospheric pressure is {weatherData.Pressure} hPa.";
    }
}
