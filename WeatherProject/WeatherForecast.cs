using System.Text.Json.Serialization;

namespace WeatherProject;

public class WeatherForecast
{
    [JsonPropertyName("pk")] public string Pk => $"ITEM#{Id}";
    [JsonPropertyName("id")] public string Id { get; set; } = default!;
    [JsonPropertyName("temperature_c")] public int TemperatureC { get; set; }
    [JsonPropertyName("temperature_f")] public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);
    [JsonPropertyName("summary")] public string? Summary { get; set; }
}