using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace TodoApi.ContextMiddleware
{
    public class ConnectionStringMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public ConnectionStringMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Extract the client identifier
            var clientId = context.Request.Headers["X-Client-ID"].ToString();

            if (string.IsNullOrEmpty(clientId) ||
                !_configuration.GetSection("ConnectionStrings:ClientDatabases").Exists())
            {
                context.Response.StatusCode = 400; // Bad Request
                await context.Response.WriteAsync("Invalid or missing client ID.");
                return;
            }

            // Retrieve the connection string
            var connectionString = _configuration.GetSection($"ConnectionStrings:ClientDatabases:{clientId}").Value;

            // Store the connection string in the HttpContext for downstream services
            context.Items["ConnectionString"] = connectionString;

            await _next(context);
        }
    }

}
