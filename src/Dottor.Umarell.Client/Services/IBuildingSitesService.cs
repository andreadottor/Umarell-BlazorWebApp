namespace Dottor.Umarell.Client.Services;

using Dottor.Umarell.Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IBuildingSitesService
{
    ValueTask<IEnumerable<BuildingSiteModel>> GetAllBuildingSiteAsync();
    ValueTask<bool> InsertBuildingSiteAsync(BuildingSiteInsertModel model);
}