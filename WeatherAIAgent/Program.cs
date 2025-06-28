using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Microsoft.SemanticKernel;
using WeatherAIAgent.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure Azure Key Vault (if URI is provided)
var keyVaultUri = builder.Configuration["Azure:KeyVault:VaultUri"];
if (!string.IsNullOrEmpty(keyVaultUri))
{
    builder.Configuration.AddAzureKeyVault(
        new Uri(keyVaultUri),
        new DefaultAzureCredential());
}

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "Weather AI Agent API", 
        Version = "v1",
        Description = "An AI-powered weather assistant that processes natural language queries and provides weather information."
    });
    
    // Include XML comments for better API documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Configure HTTP clients
builder.Services.AddHttpClient<IWeatherService, OpenWeatherMapService>();

// Configure Semantic Kernel
builder.Services.AddScoped<Kernel>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    
    var kernelBuilder = Kernel.CreateBuilder();
    
    // Configure Azure OpenAI
    var endpoint = configuration["AzureOpenAI:Endpoint"] ?? 
                   configuration["AZURE_OPENAI_ENDPOINT"] ??
                   Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
                   
    var apiKey = configuration["AzureOpenAI:ApiKey"] ?? 
                 configuration["AZURE_OPENAI_API_KEY"] ??
                 Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY");
                 
    var deploymentName = configuration["AzureOpenAI:DeploymentName"] ?? 
                         configuration["AZURE_OPENAI_DEPLOYMENT_NAME"] ??
                         Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT_NAME") ??
                         "gpt-4"; // Default fallback
    
    if (!string.IsNullOrEmpty(endpoint) && !string.IsNullOrEmpty(apiKey) && !string.IsNullOrEmpty(deploymentName))
    {
        kernelBuilder.AddAzureOpenAIChatCompletion(
            deploymentName: deploymentName,
            endpoint: endpoint,
            apiKey: apiKey);
    }
    else
    {
        // Improved error message with environment variable instructions
        throw new InvalidOperationException(@"Azure OpenAI configuration is required. Please set one of the following:

Environment Variables:
- AZURE_OPENAI_ENDPOINT: Your Azure OpenAI resource endpoint
- AZURE_OPENAI_API_KEY: Your Azure OpenAI API key
- AZURE_OPENAI_DEPLOYMENT_NAME: Your deployment name (optional, defaults to 'gpt-4')

Configuration Files:
- AzureOpenAI:Endpoint in appsettings.json
- AzureOpenAI:ApiKey in appsettings.json
- AzureOpenAI:DeploymentName in appsettings.json

Azure Key Vault:
- Configure Azure:KeyVault:VaultUri and store secrets there");
    }
    
    return kernelBuilder.Build();
});

// Register application services
builder.Services.AddScoped<IWeatherService, OpenWeatherMapService>();
builder.Services.AddScoped<IWeatherAIService, WeatherAIService>();

// Configure CORS for development
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather AI Agent API v1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root
    });
    
    app.UseCors("AllowAll");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Add a welcome endpoint
app.MapGet("/", () => new {
    message = "Welcome to Weather AI Agent API",
    documentation = "/swagger",
    endpoints = new {
        weatherQuery = "/api/weather/query",
        health = "/api/weather/health"
    }
});

app.Run();
