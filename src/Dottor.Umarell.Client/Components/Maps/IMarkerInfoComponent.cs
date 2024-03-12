﻿namespace Dottor.Umarell.Client.Components.Maps;

using Dottor.Umarell.Client.Models;
using Microsoft.AspNetCore.Components;

public interface IMarkerInfoComponent
{
    BuildingSiteModel? SelectedItem { get; set; }
    ElementReference MarkerInfoElement { get; }
    Task ButtonClickAsync(Guid buildingSiteId, string action);
}
