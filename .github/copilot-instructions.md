# Copilot Instructions for Weather AI Agent

<!-- Use this file to provide workspace-specific custom instructions to Copilot. For more details, visit https://code.visualstudio.com/docs/copilot/copilot-customization#_use-a-githubcopilotinstructionsmd-file -->

## Project Overview
This is a Weather AI Agent project built with ASP.NET Core WebAPI that integrates:
- Azure OpenAI for natural language understanding and reasoning
- Semantic Kernel for agent-like behavior
- OpenWeatherMap API for weather data retrieval
- Azure Key Vault for secure API key management

## Architecture Guidelines
- Use Semantic Kernel for orchestrating AI workflows
- Implement proper error handling and logging
- Follow RESTful API design principles
- Use dependency injection for services
- Implement proper configuration management with Azure Key Vault
- Use async/await patterns for all I/O operations

## Code Style
- Follow C# naming conventions
- Use proper XML documentation for public methods
- Implement proper validation for API inputs
- Use appropriate HTTP status codes for responses
- Include comprehensive error handling

## Security Considerations
- Never expose API keys in code or configuration files
- Use Azure Key Vault for sensitive configuration
- Implement proper input validation
- Use HTTPS for all external API calls
