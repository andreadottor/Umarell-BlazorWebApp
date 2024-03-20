namespace Dottor.Umarell.Client.Components.Maps;

using Dottor.Umarell.Client.Models;
using Microsoft.AspNetCore.Components;

public interface IMarkerInfoComponent
{
    ElementReference MarkerInfoElement { get; }
    Task ButtonClickAsync(Guid buildingSiteId, string action);

    void SetItem(BuildingSiteModel buildingSite);
}
