using Newtonsoft.Json;
using PointOfSale.Shared.Exceptions;
using PointOfSale.Shared.Models;
using System.Net;

namespace PointOfSale.Api;
public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
        _next = next;
    }

    /// <summary>
    /// Invokes all requests
    /// </summary>
    /// <param name="httpContext">The http context of the request</param>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ValidationException ex)
        {
            _logger.LogError($"Couldn't validate request", ex);
            await HandleValidationExceptionAsync(httpContext, ex);
        }
        catch (TrackingException ex)
        {
            _logger.LogError($"Tracking was not available", ex);
            await HandleTrackingExceptionAsync(httpContext, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex.Message}", ex);
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    //Prepares a bad request response when a validation error occured
    private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        await HandleCustomException(context, exception);
    }

    //Prepares a internal server error response when an error occured during tracking
    private async Task HandleTrackingExceptionAsync(HttpContext context, TrackingException exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await HandleCustomException(context, exception);
    }

    //Sets the response
    private async Task HandleCustomException(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var result = new ErrorDetails()
        {
            StatusCode = context.Response.StatusCode,
            Message = exception.Message
        };

        await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
    }

    //Prepares and sets the internal server error response for all other exceptions
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var result = new ErrorDetails()
        {
            StatusCode = context.Response.StatusCode,
            Message = $"An exception (Type:'{exception.GetType()}') occured while processing the request. Please try again later or contact the admin."
        };

        await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
    }
}