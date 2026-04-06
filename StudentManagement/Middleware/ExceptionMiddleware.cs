using Microsoft.Data.SqlClient;

public class ExceptionMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ExceptionMiddleware> _logger;

	public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An exception occurred: {Message}", ex.Message);
			await HandleExceptionAsync(context, ex);
		}
	}

	private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		context.Response.ContentType = "application/json";

		var (statusCode, message) = exception switch
		{
			ArgumentException ex => (StatusCodes.Status400BadRequest, ex.Message),
			KeyNotFoundException ex => (StatusCodes.Status404NotFound, ex.Message),
			UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized access."),
			SqlException => (StatusCodes.Status503ServiceUnavailable, "Database error occurred."),
			_ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
		};

		context.Response.StatusCode = statusCode;

		await context.Response.WriteAsJsonAsync(new
		{
			StatusCode = statusCode,
			Message = message
		});
	}
}