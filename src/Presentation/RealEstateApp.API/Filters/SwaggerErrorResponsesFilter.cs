using Microsoft.OpenApi.Models;
using RealEstateApp.API.Filters.Helpers;
using RealEstateApp.API.Middlewares;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RealEstateApp.API.Filters;

public class SwaggerErrorResponsesFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var errorSchema = context.SchemaGenerator.GenerateSchema(typeof(ErrorResponse), context.SchemaRepository);

        OpenApiResponseFactory.TryAdd(operation, "400", OpenApiResponseFactory.CreateJsonResponse("Bad Request",           errorSchema));
        OpenApiResponseFactory.TryAdd(operation, "401", new OpenApiResponse { Description = "Unauthorized" });
        OpenApiResponseFactory.TryAdd(operation, "403", new OpenApiResponse { Description = "Forbidden" });
        OpenApiResponseFactory.TryAdd(operation, "404", OpenApiResponseFactory.CreateJsonResponse("Not Found",             errorSchema));
        OpenApiResponseFactory.TryAdd(operation, "500", OpenApiResponseFactory.CreateJsonResponse("Internal Server Error", errorSchema));
    }
}
