namespace Dottor.Umarell.Client.Services;

using System.Text.Json;
using System.Text.Json.Serialization;

public class WeatherProxyService : IWeatherProxyService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WeatherProxyService> _logger;

    public WeatherProxyService(HttpClient httpClient, ILogger<WeatherProxyService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<float?> GetCurrentTemperatureAsync(double latitude, double longitude)
    {
        await Task.Delay(3000);
        try
        {
            var response = await _httpClient.GetAsync($"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current=temperature");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<OpenMeteoResponse>(content, OpenMeteoSerializerOptions);
            return result?.Current?.Temperature;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error on retrieve current temperature.");
            return null;
        }
    }

    private static JsonSerializerOptions OpenMeteoSerializerOptions => new() { PropertyNameCaseInsensitive = true };

    public class OpenMeteoResponse
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        [JsonPropertyName("generationtime_ms")]
        public float GenerationTimeMs { get; set; }
        [JsonPropertyName("utc_offset_seconds")]
        public int UtcOffsetSeconds { get; set; }
        public string Timezone { get; set; } = default!;
        [JsonPropertyName("timezone_abbreviation")]
        public string TimezoneAbbreviation { get; set; } = default!;
        public int Elevation { get; set; }
        [JsonPropertyName("current_units")]
        public CurrentUnits? CurrentUnits { get; set; }
        public Current? Current { get; set; }
    }

    public class CurrentUnits
    {
        public string Time { get; set; } = default!;
        public string Interval { get; set; } = default!;
        public string Temperature { get; set; } = default!;
    }

    public class Current
    {
        public DateTime Time { get; set; } = default!;
        public int Interval { get; set; }
        public float Temperature { get; set; }
    }


}
