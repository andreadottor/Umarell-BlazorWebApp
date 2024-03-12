namespace Dottor.Umarell.Services;

using Dottor.Umarell.Client.Models;
using Dottor.Umarell.Client.Services;
using Dottor.Umarell.Data;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class BuildingSitesDataService : IBuildingSitesService
{
    private readonly string _filesFolder;
    private readonly ApplicationDbContext _db;
    private readonly ILogger<BuildingSitesDataService> _logger;

    public BuildingSitesDataService(IConfiguration configuration,
                                    ApplicationDbContext db,
                                    ILogger<BuildingSitesDataService> logger)
    {
        _filesFolder = configuration.GetValue<string>("FilesFolder") ?? "umarell-images";
        _db = db;
        _logger = logger;
    }

    public async ValueTask<IEnumerable<BuildingSiteModel>> GetAllBuildingSiteAsync()
    {
        var sites = await _db.BuildingSites.ToListAsync();
        var list = new List<BuildingSiteModel>();
        foreach (var item in sites)
        {
            list.Add(new()
            {
                FileName = item.FileName,
                Id = item.Id,
                Latitude = item.Latitude,
                Longitude = item.Longitude,
                StartDate = item.StartDate,
                Title = item.Title
            });
        }
        return list;
    }

    public async ValueTask<bool> InsertBuildingSiteAsync(BuildingSiteInsertModel model)
    {
        try
        {
            BuildingSite buildingSite = new()
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                FileName = model.FileName,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                StartDate = model.StartDate
            };

            if (!string.IsNullOrWhiteSpace(model.FileName))
            {
                var provider = new FileExtensionContentTypeProvider();
                string contentType;
                if (!provider.TryGetContentType(model.FileName, out contentType))
                {
                    contentType = "application/octet-stream";
                }
                buildingSite.FileContentType = contentType;

                var folderPath = Path.Combine(_filesFolder, buildingSite.Id.ToString());
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var path = Path.Combine(_filesFolder, buildingSite.Id.ToString(), model.FileName);
                await System.IO.File.WriteAllBytesAsync(path, model.FileContent);

                buildingSite.FilePath = Path.Combine(buildingSite.Id.ToString(), model.FileName);
            }

            _db.BuildingSites.Add(buildingSite);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during insert building site");
            return false;
        }
    }
}
