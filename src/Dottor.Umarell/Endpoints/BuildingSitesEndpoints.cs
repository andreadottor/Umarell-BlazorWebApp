namespace Dottor.Umarell.Endpoints;

using Dottor.Umarell.Client.Models;
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
             .WithSummary("Get all BuildingSite")
             .WithDescription("Return all Building Site.");

        group.MapGet("image/{buildingSiteId:guid}", GetBuildingSiteImageAsync)
             .WithName("GetBuildingSiteImageAsync")
             .WithSummary("Get BuildingSite photo")
             .WithDescription("Return the Building Site photo if is present.");

        //group.MapPost("", CreateReviewAsync)
        //    .WithName("CreateReviewAsync")
        //     .WithSummary("Create beer review")
        //     .WithDescription("Create a new review for the specified beer");

        return endpoints;
    }

    private static Results<Ok<IEnumerable<BuildingSiteModel>>, ProblemHttpResult> GetBuildingSitesAsync(ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger(nameof(BuildingSitesEndpoints));
        try
        {
            IEnumerable<BuildingSiteModel> list = new List<BuildingSiteModel>();
            //var query = new GetReviews(beerId);
            //var list = service.Execute(query);
            //return TypedResults.Ok(list);
            return TypedResults.Ok(list);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on retrieve Building Site list.");
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
