using System.Text;
using System.Text.Json;

namespace WeatherAIAgent.Examples;

/// <summary>
/// Example client demonstrating how to interact with the Weather AI Agent API
/// </summary>
public class WeatherApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public WeatherApiClient(string baseUrl = "https://localhost:7000")
    {
        _httpClient = new HttpClient();
        _baseUrl = baseUrl;
    }

    /// <summary>
    /// Sends a weather query to the AI agent
    /// </summary>
    public async Task<string> QueryWeatherAsync(string query)
    {
        var request = new { Query = query };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/weather/query", content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<JsonElement>(responseContent);
                
                if (result.TryGetProperty("response", out var responseProperty))
                {
                    return responseProperty.GetString() ?? "No response received";
                }
            }
            else
            {
                return $"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
            }
        }
        catch (Exception ex)
        {
            return $"Exception: {ex.Message}";
        }

        return "Failed to get response";
    }

    /// <summary>
    /// Checks if the weather service is healthy
    /// </summary>
    public async Task<bool> CheckHealthAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/weather/health");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}

/// <summary>
/// Console application demonstrating the Weather AI Agent
/// </summary>
public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("🌤️  Weather AI Agent Demo");
        Console.WriteLine("========================");
        Console.WriteLine();

        var client = new WeatherApiClient();

        // Check if the service is running
        Console.WriteLine("Checking service health...");
        var isHealthy = await client.CheckHealthAsync();
        
        if (!isHealthy)
        {
            Console.WriteLine("❌ Weather AI Agent service is not running.");
            Console.WriteLine("Please start the service first by running: dotnet run");
            return;
        }

        Console.WriteLine("✅ Service is healthy!");
        Console.WriteLine();

        // Example queries
        var exampleQueries = new[]
        {
            "What's the weather in New York?",
            "How's the weather in London today?",
            "Tell me about the current weather in Tokyo, Japan",
            "Is it raining in Paris right now?",
            "What's the temperature in Sydney?"
        };

        Console.WriteLine("Example Queries:");
        Console.WriteLine("================");

        foreach (var query in exampleQueries)
        {
            Console.WriteLine($"\n🔍 Query: \"{query}\"");
            Console.WriteLine("🤖 AI Response:");
            
            var response = await client.QueryWeatherAsync(query);
            Console.WriteLine($"   {response}");
            
            // Add a small delay between requests
            await Task.Delay(1000);
        }

        Console.WriteLine("\n" + new string('=', 50));
        Console.WriteLine("Interactive Mode:");
        Console.WriteLine("Type your weather questions (or 'quit' to exit)");
        Console.WriteLine(new string('=', 50));

        while (true)
        {
            Console.Write("\n🌤️  Ask about weather: ");
            var userQuery = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userQuery) || userQuery.ToLower() == "quit")
                break;

            Console.WriteLine("🤖 AI Response:");
            var response = await client.QueryWeatherAsync(userQuery);
            Console.WriteLine($"   {response}");
        }

        Console.WriteLine("\nThank you for using Weather AI Agent! 👋");
        client.Dispose();
    }
}
