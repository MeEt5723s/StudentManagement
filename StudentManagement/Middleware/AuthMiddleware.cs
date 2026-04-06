public class AuthMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<AuthMiddleware> _logger;

	// Routes that skip auth checks
	private static readonly string[] _publicRoutes =
	[
		"/api/auth/login",
		"/swagger"
	];

	public AuthMiddleware(RequestDelegate next, ILogger<AuthMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		var path = context.Request.Path.Value?.ToLower();

		// Skip auth for public routes
		if (_publicRoutes.Any(r => path != null && path.StartsWith(r)))
		{
			await _next(context);
			return;
		}

		// Check if user is authenticated (JWT middleware already validated the token)
		if (!context.User.Identity?.IsAuthenticated ?? true)
		{
			_logger.LogWarning("Unauthorized access attempt to {Path} from {IP}",
				context.Request.Path,
				context.Connection.RemoteIpAddress);

			context.Response.StatusCode = StatusCodes.Status401Unauthorized;
			context.Response.ContentType = "application/json";

			await context.Response.WriteAsJsonAsync(new
			{
				StatusCode = 401,
				Message = "Unauthorized. Please provide a valid token."
			});

			return;
		}

		// Attach user info to logs
		var userId = context.User.FindFirst("id")?.Value;
		var userName = context.User.Identity?.Name;

		_logger.LogInformation("Authenticated request by User: {UserName} (ID: {UserId}) to {Path}",
			userName, userId, context.Request.Path);

		await _next(context);
	}
}