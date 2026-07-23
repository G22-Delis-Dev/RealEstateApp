using System.Net;
using System.Text.Json;
using RealEstateApp.Application.Common.Exceptions;
using RealEstateApp.Domain.Exceptions;

namespace RealEstateApp.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse();

        switch (exception)
        {
            case NotFoundException notFoundEx:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound; // 404
                response.StatusCode = 404;
                response.Message = notFoundEx.Message;
                break;

            case ValidationException validationEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400
                response.StatusCode = 400;
                response.Message = validationEx.Message;
                response.Errors = validationEx.Errors;
                break;

            case BusinessRuleValidationException businessRuleEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400
                response.StatusCode = 400;
                response.Message = businessRuleEx.Message;
                break;

            case ForbiddenAccessException forbiddenEx:
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden; // 403
                response.StatusCode = 403;
                response.Message = forbiddenEx.Message;
                break;

            case ConflictException conflictEx:
                context.Response.StatusCode = (int)HttpStatusCode.Conflict; // 409
                response.StatusCode = 409;
                response.Message = conflictEx.Message;
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500
                response.StatusCode = 500;
                response.Message = "Ha ocurrido un error interno del servidor.";
                Console.WriteLine($"[Unhandled Exception]: {exception}");
                break;
        }

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(response, jsonOptions);
        await context.Response.WriteAsync(json);
    }
}

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = null!;
    public object? Errors { get; set; }
}
