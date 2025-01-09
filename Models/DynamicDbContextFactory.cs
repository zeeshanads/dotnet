using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using TodoApi.Services;

namespace TodoApi.Models
{

    public class DynamicDbContextFactory 
    {
        private readonly ITenantService _tenantService;

        public DynamicDbContextFactory(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }


        public TodoContext CreateDbContext()
        {
            var connectionString = _tenantService.GetConnectionString();

            var optionsBuilder = new DbContextOptionsBuilder<TodoContext>();
           
            //TODO: here need to set the connection string for the database
            //optionsBuilder.UseSqlServer(connectionString); 
            //optionsBuilder.UseSqlite(connectionString);

            return new TodoContext(optionsBuilder.Options);
        }
    }
}
