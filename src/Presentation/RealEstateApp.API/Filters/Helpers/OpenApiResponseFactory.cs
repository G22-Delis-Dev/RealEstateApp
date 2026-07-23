using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RealEstateApp.API.Filters.Helpers;

internal static class OpenApiResponseFactory
{
    internal static Type UnwrapTaskType(Type type) =>
        type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>)
            ? type.GetGenericArguments()[0]
            : type;

    internal static Type? ExtractDtoType(Type type) =>
        type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ActionResult<>)
            ? type.GetGenericArguments()[0]
            : null;

    internal static bool IsCollectionType(Type type) =>
        type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);

    internal static OpenApiResponse CreateJsonResponse(string description, Type dtoType, OperationFilterContext context)
    {
        var schema = context.SchemaGenerator.GenerateSchema(dtoType, context.SchemaRepository);
        return CreateJsonResponse(description, schema);
    }

    internal static OpenApiResponse CreateJsonResponse(string description, OpenApiSchema schema) =>
        new()
        {
            Description = description,
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType { Schema = schema }
            }
        };

    internal static void TryAdd(OpenApiOperation operation, string statusCode, OpenApiResponse response)
    {
        if (!operation.Responses.ContainsKey(statusCode))
            operation.Responses.Add(statusCode, response);
    }
}
