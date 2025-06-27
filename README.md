# Weather AI Agent

A sophisticated AI-powered weather assistant built with ASP.NET Core WebAPI, Azure OpenAI, and Semantic Kernel. This application processes natural language weather queries and provides intelligent responses using external weather APIs.

## 🌟 Features

- **Natural Language Processing**: Uses Azure OpenAI to understand and process natural language weather queries
- **Intelligent Agent**: Powered by Microsoft Semantic Kernel for agent-like reasoning and function calling
- **Real-time Weather Data**: Integrates with OpenWeatherMap API for current weather information
- **Secure Configuration**: Azure Key Vault integration for secure API key management
- **RESTful API**: Clean, documented API endpoints with Swagger/OpenAPI support
- **Production Ready**: Comprehensive logging, error handling, and CORS support

## 🏗️ Architecture

```
User Query → Azure OpenAI → Semantic Kernel → Weather API → Structured Response
```

### Technology Stack

- **Backend**: ASP.NET Core 8.0 WebAPI
- **AI/ML**: Azure OpenAI, Microsoft Semantic Kernel
- **Weather Data**: OpenWeatherMap API
- **Security**: Azure Key Vault, Azure Identity
- **Documentation**: Swagger/OpenAPI
- **Serialization**: Newtonsoft.Json

## 🚀 Getting Started

### Prerequisites

1. **.NET 8.0 SDK** or later
2. **Azure OpenAI Service** with a deployed GPT-4 model
3. **OpenWeatherMap API** account and API key
4. **Azure Key Vault** (optional, for production)

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd WeatherAIAgent
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure API Keys**

   Update `appsettings.Development.json`:
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

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access the API**
   - Swagger UI: `https://localhost:7xxx` (automatically opens)
   - API Base URL: `https://localhost:7xxx/api`

## 📖 Usage

### API Endpoints

#### POST `/api/weather/query`
Process a natural language weather query.

**Request Body:**
```json
{
  "query": "What's the weather like in Tokyo today?"
}
```

**Response:**
```json
{
  "response": "The current weather in Tokyo, Japan is clear sky with a temperature of 22.5°C (feels like 23.1°C). Humidity is 65%, wind speed is 3.2 m/s, and atmospheric pressure is 1013 hPa.",
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

#### GET `/api/weather/health`
Check service health status.

### Example Queries

The AI agent can understand various natural language patterns:

- "What's the weather in New York?"
- "How's the weather in London today?"
- "Tell me about the current weather in Tokyo, Japan"
- "Is it raining in Paris right now?"
- "What's the temperature in Sydney?"

## 🛠️ Configuration

### Environment Variables

For production deployments, use environment variables or Azure Key Vault:

```bash
AzureOpenAI__Endpoint=https://your-resource.openai.azure.com/
AzureOpenAI__ApiKey=your-key
AzureOpenAI__DeploymentName=gpt-4
OpenWeatherMap__ApiKey=your-key
```

### Azure Key Vault Integration

1. Set up Azure Key Vault
2. Add secrets:
   - `AzureOpenAI--ApiKey`
   - `OpenWeatherMap--ApiKey`
3. Configure in `appsettings.json`:
   ```json
   {
     "Azure": {
       "KeyVault": {
         "VaultUri": "https://your-keyvault.vault.azure.net/"
       }
     }
   }
   ```

## 🔧 Development

### Project Structure

```
WeatherAIAgent/
├── Controllers/
│   └── WeatherController.cs          # API endpoints
├── Models/
│   ├── WeatherModels.cs              # Domain models
│   └── OpenWeatherMapModels.cs       # External API models
├── Services/
│   ├── IWeatherService.cs            # Weather service interface
│   ├── OpenWeatherMapService.cs      # Weather data provider
│   ├── IWeatherAIService.cs          # AI service interface
│   └── WeatherAIService.cs           # AI agent implementation
├── Program.cs                        # Application configuration
├── appsettings.json                  # Configuration
└── appsettings.Development.json      # Development configuration
```

### Adding New Features

1. **New Weather Providers**: Implement `IWeatherService`
2. **Additional AI Functions**: Add methods to `WeatherPlugin`
3. **Enhanced Responses**: Modify response models in `Models/`

### Running Tests

```bash
dotnet test
```

## 🚀 Deployment

### Azure App Service

1. Create Azure App Service
2. Configure Application Settings with API keys
3. Deploy using Visual Studio or Azure CLI

### Docker

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WeatherAIAgent.csproj", "."]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherAIAgent.dll"]
```

## 🔐 Security

- **API Keys**: Never commit API keys to source control
- **Azure Key Vault**: Use for production secret management
- **HTTPS**: All external API calls use HTTPS
- **Input Validation**: Comprehensive request validation
- **Error Handling**: Secure error responses without sensitive data

## 📝 API Documentation

Once running, visit the Swagger UI at the application root for interactive API documentation.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make changes following the coding guidelines in `.github/copilot-instructions.md`
4. Submit a pull request

## 📄 License

This project is licensed under the MIT License.

## 🆘 Troubleshooting

### Common Issues

1. **Azure OpenAI Connection Failed**
   - Verify endpoint URL and API key
   - Check deployment name matches configuration
   - Ensure API version is supported

2. **Weather API Errors**
   - Verify OpenWeatherMap API key
   - Check API usage limits
   - Validate city names

3. **Semantic Kernel Issues**
   - Ensure compatible package versions
   - Check AI model deployment status

### Support

For issues and questions, please create an issue in the repository.
