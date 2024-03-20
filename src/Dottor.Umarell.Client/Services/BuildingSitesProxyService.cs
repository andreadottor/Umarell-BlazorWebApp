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

    public async Task<bool> InsertBuildingSiteAsync(BuildingSiteInsertModel model)
    {
        var response = await _httpClient.PostAsJsonAsync($"/api/v1/building-sites/", model);
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<BuildingSiteModel>> GetAllBuildingSiteAsync()
    {
        var response = await _httpClient.GetAsync($"/api/v1/building-sites/");
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<ApiResult<IEnumerable<BuildingSiteModel>>>();
            if (data is not null && data.Result)
                return data.Data ?? [];
        }

        throw new Exception("Error on retrieve list of BuildingSites");
    }


    public async Task<int> BuildingSitesCountAsync()
    {
        var response = await _httpClient.GetAsync($"/api/v1/building-sites/count");
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<ApiResult<int>>();
            if (data is not null && data.Result)
                return data.Data;
        }

        throw new Exception("Error on retrieve the number of BuildingSites");
    }

}
