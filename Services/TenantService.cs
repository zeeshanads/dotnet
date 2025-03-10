using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;

namespace TodoApi.Services
{
    public interface ITenantService
    {
        string GetConnectionString();
    }
    public class TenantService : ITenantService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public TenantService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public string GetConnectionString()
        {
            // Extract client ID from header
            var clientId = _httpContextAccessor.HttpContext?.Request.Headers["X-Client-ID"].ToString();

            if (string.IsNullOrEmpty(clientId))
                throw new UnauthorizedAccessException("Client ID is missing.");

            // Retrieve the connection string from configuration
            var connectionString = _configuration.GetSection($"ConnectionStrings:ClientDatabases:{clientId}").Value;

            if (string.IsNullOrEmpty(connectionString))
                throw new UnauthorizedAccessException("Invalid client ID or connection string not found.");

            return connectionString;
        }

    }
}