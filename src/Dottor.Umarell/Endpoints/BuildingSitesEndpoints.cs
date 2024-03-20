namespace Dottor.Umarell.Endpoints;

using Dottor.Umarell.Client.Models;
using Dottor.Umarell.Client.Services;
using Dottor.Umarell.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections;
using System.Collections.Generic;

public static class BuildingSitesEndpoints
{
    public static IEndpointRouteBuilder MapBuildingSitesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/v1/building-sites/");
        group.WithTags("Public");
        group.WithOpenApi();

        group.MapGet("", GetBuildingSitesAsync)
             .WithName("GetBuildingSitesAsync")
             .WithSummary("Get all Building Sites");

        group.MapGet("image/{buildingSiteId:guid}", GetBuildingSiteImageAsync)
             .WithName("GetBuildingSiteImageAsync")
             .WithSummary("Get BuildingSite photo");

        group.MapGet("/count", GetBuildingSitesCountAsync)
             .WithName("GetBuildingSitesCountAsync")
             .WithSummary("Return the nmber of Building Sites");

        group.MapPost("", CreateBuildingSiteAsync)
            .WithName("CreateBuildingSiteAsync")
             .WithSummary("Create new BuildingSite");

        return endpoints;
    }

    private static async Task<Results<Ok<ApiResult<IEnumerable<BuildingSiteModel>>>, ProblemHttpResult>> GetBuildingSitesAsync(
            ILoggerFactory loggerFactory, 
            IBuildingSitesService buildingSitesService)
    {
        var logger = loggerFactory.CreateLogger(nameof(BuildingSitesEndpoints));
        try
        {
            IEnumerable<BuildingSiteModel> list = await buildingSitesService.GetAllBuildingSiteAsync();
            var result = new ApiResult<IEnumerable<BuildingSiteModel>> 
            { 
                Data = list, 
                Result = true 
            };
            return TypedResults.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on retrieve Building Site list.");
            return TypedResults.Problem(ex.Message, title: "Error on BuildingSites API");
        }
    }

    private static async Task<Results<Created, ProblemHttpResult>> CreateBuildingSiteAsync(
            BuildingSiteInsertModel model,
            ILoggerFactory loggerFactory,
            IBuildingSitesService buildingSitesService)
    {
        var logger = loggerFactory.CreateLogger(nameof(BuildingSitesEndpoints));
        try
        {
            await buildingSitesService.InsertBuildingSiteAsync(model);
            return TypedResults.Created();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on insert new Building Site.");
            return TypedResults.Problem(ex.Message, title: "Error on BuildingSites API");
        }
    }

    private static async Task<Results<Ok<ApiResult<int>>, ProblemHttpResult>> GetBuildingSitesCountAsync(
                ILoggerFactory loggerFactory,
                IBuildingSitesService buildingSitesService)
    {
        var logger = loggerFactory.CreateLogger(nameof(BuildingSitesEndpoints));
        try
        {
            int count = await buildingSitesService.BuildingSitesCountAsync();
            return TypedResults.Ok(new ApiResult<int> { Data = count, Result = true });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on retrieve Building Site count.");
            return TypedResults.Problem(ex.Message, title: "Error on BuildingSites API");
        }
    }

    private static async Task<Results<FileStreamHttpResult, NotFound>> GetBuildingSiteImageAsync(Guid buildingSiteId,
                                                                                               ILoggerFactory loggerFactory,
                                                                                               IConfiguration configuration,
                                                                                               ApplicationDbContext db)
    {
        var logger = loggerFactory.CreateLogger(nameof(BuildingSitesEndpoints));
        var filesFolder = configuration.GetValue<string>("FilesFolder") ?? "umarell-images";

        if (await db.BuildingSites.FindAsync(buildingSiteId) is BuildingSite buildingSite)
        {
            if (!string.IsNullOrWhiteSpace(buildingSite.FilePath))
            {
                var path = Path.Combine(filesFolder, buildingSite.FilePath);

                if (System.IO.File.Exists(path))
                {
                    var image = System.IO.File.OpenRead(path);
                    return TypedResults.File(image, contentType: buildingSite.FileContentType);
                }
            }
        }

        return TypedResults.NotFound();
    }

}
