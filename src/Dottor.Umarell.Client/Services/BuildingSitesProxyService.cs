namespace Dottor.Umarell.Client.Services;

using Dottor.Umarell.Client.Models;
using System.Net.Http.Json;

public class BuildingSitesProxyService : IBuildingSitesService
{
    private readonly HttpClient _httpClient;

    public BuildingSitesProxyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async ValueTask<bool> InsertBuildingSiteAsync(BuildingSiteInsertModel model)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/v1/BuildingSites", model);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<ApiResult>();
            if (data is not null)
                return data.Result;
        }

        return false;
    }

    public async ValueTask<IEnumerable<BuildingSiteModel>> GetAllBuildingSiteAsync()
    {
        var response = await _httpClient.GetAsync($"api/v1/BuildingSites");
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<ApiResult<IEnumerable<BuildingSiteModel>>>();
            if (data is not null && data.Result)
                return data.Data ?? [];
        }

        throw new Exception("Error on retrieve list of BuildingSites");
    }

}
