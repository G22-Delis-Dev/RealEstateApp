using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using RealEstateApp.API.Filters.Helpers;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RealEstateApp.API.Filters;

public class SwaggerSuccessResponsesFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var httpMethod = context.ApiDescription.HttpMethod?.ToUpperInvariant();
        var returnType = OpenApiResponseFactory.UnwrapTaskType(context.MethodInfo.ReturnType);
        var dtoType    = OpenApiResponseFactory.ExtractDtoType(returnType);

        switch (httpMethod)
        {
            case "GET":
                if (dtoType is not null)
                    OpenApiResponseFactory.TryAdd(operation, "200",
                        OpenApiResponseFactory.CreateJsonResponse("OK", dtoType, context));

                if (dtoType is not null && OpenApiResponseFactory.IsCollectionType(dtoType))
                    OpenApiResponseFactory.TryAdd(operation, "204",
                        new OpenApiResponse { Description = "No Content" });
                break;

            case "POST":
                if (dtoType is not null)
                    OpenApiResponseFactory.TryAdd(operation, "201",
                        OpenApiResponseFactory.CreateJsonResponse("Created", dtoType, context));
                break;

            case "PUT":
                OpenApiResponseFactory.TryAdd(operation, "200",
                    new OpenApiResponse { Description = "OK" });
                break;

            case "PATCH":
            case "DELETE":
                OpenApiResponseFactory.TryAdd(operation, "204",
                    new OpenApiResponse { Description = "No Content" });
                break;
        }
    }
}
