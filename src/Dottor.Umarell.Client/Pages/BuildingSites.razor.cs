namespace Dottor.Umarell.Client.Pages;

using Dottor.Umarell.Client.Models;

public partial class BuildingSites
{
    private IEnumerable<BuildingSiteModel>? items;

    protected override async Task OnInitializedAsync()
    {
        items = await BuildingSitesService.GetAllBuildingSiteAsync();
    }
}
