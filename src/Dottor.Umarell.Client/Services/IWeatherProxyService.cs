namespace Dottor.Umarell.Client.Services;

using System.Threading.Tasks;

public interface IWeatherProxyService
{
    Task<float?> GetCurrentTemperatureAsync(double latitude, double longitude);
}