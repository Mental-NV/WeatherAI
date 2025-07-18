using Newtonsoft.Json;

namespace WeatherAIAgent.Models;

/// <summary>
/// OpenWeatherMap API response models
/// </summary>
public class OpenWeatherMapResponse
{
    [JsonProperty("coord")]
    public Coordinates? Coord { get; set; }

    [JsonProperty("weather")]
    public Weather[]? Weather { get; set; }

    [JsonProperty("base")]
    public string? Base { get; set; }

    [JsonProperty("main")]
    public MainWeatherData? Main { get; set; }

    [JsonProperty("visibility")]
    public int Visibility { get; set; }

    [JsonProperty("wind")]
    public Wind? Wind { get; set; }

    [JsonProperty("clouds")]
    public Clouds? Clouds { get; set; }

    [JsonProperty("dt")]
    public long Dt { get; set; }

    [JsonProperty("sys")]
    public SystemData? Sys { get; set; }

    [JsonProperty("timezone")]
    public int Timezone { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("cod")]
    public int Cod { get; set; }
}

public class Coordinates
{
    [JsonProperty("lon")]
    public double Lon { get; set; }

    [JsonProperty("lat")]
    public double Lat { get; set; }
}

public class Weather
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("main")]
    public string? Main { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("icon")]
    public string? Icon { get; set; }
}

public class MainWeatherData
{
    [JsonProperty("temp")]
    public double Temp { get; set; }

    [JsonProperty("feels_like")]
    public double FeelsLike { get; set; }

    [JsonProperty("temp_min")]
    public double TempMin { get; set; }

    [JsonProperty("temp_max")]
    public double TempMax { get; set; }

    [JsonProperty("pressure")]
    public int Pressure { get; set; }

    [JsonProperty("humidity")]
    public int Humidity { get; set; }
}

public class Wind
{
    [JsonProperty("speed")]
    public double Speed { get; set; }

    [JsonProperty("deg")]
    public int Deg { get; set; }
}

public class Clouds
{
    [JsonProperty("all")]
    public int All { get; set; }
}

public class SystemData
{
    [JsonProperty("type")]
    public int Type { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("country")]
    public string? Country { get; set; }

    [JsonProperty("sunrise")]
    public long Sunrise { get; set; }

    [JsonProperty("sunset")]
    public long Sunset { get; set; }
}
