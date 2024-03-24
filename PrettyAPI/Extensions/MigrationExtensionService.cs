using DataLayer;
using Entities;
using Entities.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;

namespace PrettyAPI.Extensions
{
    public static class MigrationExtensionService
    {
        public static IServiceCollection AutoDatabaseMigration(this IServiceCollection services, IConfiguration configuration)
        {
            // Retrieve tenant configurations from appsettings.json or any other configuration source
            var defaultConnectionString = "";

            var tenants = configuration.GetSection("TenantSettings").GetChildren();
            foreach (var tenant in tenants)
            {
                string connectionString = "";
                if (tenant.Key == "Defaults" && !string.IsNullOrEmpty(tenant["ConnectionString"]))
                {
                    defaultConnectionString = tenant["ConnectionString"];
                }

                if (string.IsNullOrEmpty(tenant["ConnectionString"]))
                {
                    connectionString = defaultConnectionString;
                }
                else
                {
                    connectionString = tenant["ConnectionString"];
                }

                var provider = tenant["DbProvider"]==null?"MSSQL": tenant["DbProvider"];
                var migrationAssembly = "PrettyAPI";

                if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(provider))
                {
                    throw new ArgumentException("ConnectionString and Provider are required for each tenant.");
                }

                services.AddDbContext<RepositoryDBContext>(options =>
                {
                    if (provider.Equals("MSSQL", StringComparison.OrdinalIgnoreCase))
                    {
                        options.UseSqlServer(connectionString, builder =>
                        {
                            builder.MigrationsAssembly(migrationAssembly);
                        });
                    }
                    // Add configurations for other providers if needed
                });
                
                using var serviceProvider = services.BuildServiceProvider();
                var dbContext = serviceProvider.GetRequiredService<RepositoryDBContext>();
                dbContext.Database.Migrate();
            }

            return services;
        }
        
    }
}
