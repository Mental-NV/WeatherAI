# Weather AI Agent Setup and Testing Guide

## 🚀 Quick Start Instructions

### 1. Configure API Keys

Before running the application, you need to configure your API keys:

1. **Get an OpenWeatherMap API Key:**
   - Visit: https://openweathermap.org/api
   - Sign up for a free account
   - Get your API key from the dashboard

2. **Set up Azure OpenAI:**
   - Create an Azure OpenAI resource in Azure Portal
   - Deploy a GPT-4 model
   - Note down your endpoint, API key, and deployment name

3. **Update Configuration:**
   Edit `appsettings.Development.json`:
   ```json
   {
     "AzureOpenAI": {
       "Endpoint": "https://your-openai-resource.openai.azure.com/",
       "ApiKey": "your-azure-openai-api-key",
       "DeploymentName": "gpt-4",
       "ApiVersion": "2024-02-15-preview"
     },
     "OpenWeatherMap": {
       "ApiKey": "your-openweathermap-api-key",
       "BaseUrl": "https://api.openweathermap.org/data/2.5"
     }
   }
   ```

### 2. Run the Application

```bash
# Build the project
dotnet build

# Run the application
dotnet run

# Or run with specific profile
dotnet run --launch-profile https
```

The application will start and be available at:
- HTTPS: https://localhost:7000
- HTTP: http://localhost:5000
- Swagger UI: https://localhost:7000/swagger

### 3. Test the API

#### Using curl:

```bash
# Test health endpoint
curl -X GET "https://localhost:7000/api/weather/health" -k

# Test weather query
curl -X POST "https://localhost:7000/api/weather/query" \
  -H "Content-Type: application/json" \
  -d '{"query": "What'\''s the weather in New York?"}' \
  -k
```

#### Using PowerShell:

```powershell
# Test health endpoint
Invoke-RestMethod -Uri "https://localhost:7000/api/weather/health" -Method GET -SkipCertificateCheck

# Test weather query
$body = @{
    query = "What's the weather in Tokyo?"
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:7000/api/weather/query" -Method POST -Body $body -ContentType "application/json" -SkipCertificateCheck
```

### 4. Example Queries to Try

- "What's the weather in New York?"
- "How's the weather in London today?"
- "Tell me about the current weather in Tokyo, Japan"
- "Is it raining in Paris right now?"
- "What's the temperature in Sydney?"

### 5. Expected Response Format

```json
{
  "response": "The current weather in Tokyo, Japan is clear sky with a temperature of 22.5°C...",
  "weatherData": {
    "city": "Tokyo",
    "country": "JP",
    "temperature": 22.5,
    "feelsLike": 23.1,
    "description": "clear sky",
    "humidity": 65,
    "windSpeed": 3.2,
    "pressure": 1013,
    "timestamp": "2025-06-27T10:30:00Z"
  },
  "success": true,
  "errorMessage": null
}
```

## 🔧 Troubleshooting

### Common Issues:

1. **"Azure OpenAI configuration is required" error:**
   - Ensure you've updated the API keys in appsettings.Development.json
   - Verify your Azure OpenAI endpoint and deployment name

2. **"Could not find weather information" error:**
   - Check your OpenWeatherMap API key
   - Verify the city name is spelled correctly
   - Ensure you haven't exceeded API rate limits

3. **SSL Certificate errors:**
   - Add `-k` flag to curl commands
   - Use `-SkipCertificateCheck` with PowerShell
   - Or configure proper SSL certificates for production

### Development Tips:

1. **View logs:**
   - Check the console output for detailed logs
   - Logs include AI processing steps and API calls

2. **Debug mode:**
   - The application runs in debug mode by default
   - Use Visual Studio or VS Code debugger for step-through debugging

3. **API Documentation:**
   - Visit `/swagger` for interactive API documentation
   - All endpoints are documented with examples

## 🚀 Next Steps

1. **Deploy to Azure:**
   - Use Azure App Service for hosting
   - Configure Azure Key Vault for production secrets
   - Set up Application Insights for monitoring

2. **Extend Functionality:**
   - Add weather forecasting capabilities
   - Implement caching for API responses
   - Add support for multiple weather providers

3. **Security Hardening:**
   - Implement API key authentication
   - Add rate limiting
   - Configure proper CORS policies

## 📚 Additional Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Azure OpenAI Service](https://azure.microsoft.com/services/cognitive-services/openai-service/)
- [Semantic Kernel Documentation](https://learn.microsoft.com/semantic-kernel/)
- [OpenWeatherMap API](https://openweathermap.org/api)
