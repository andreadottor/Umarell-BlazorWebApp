namespace Dottor.Umarell.Client.Services;

using Dottor.Umarell.Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IBuildingSitesService
{
    Task<IEnumerable<BuildingSiteModel>> GetAllBuildingSiteAsync();
    Task<int> BuildingSitesCountAsync();
    Task<bool> InsertBuildingSiteAsync(BuildingSiteInsertModel model);
}