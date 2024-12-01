using DataLayer;
using Entities;
using Entities.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Linq;

namespace PrettyAPI.Extensions
{
    public static class MigrationExtensionService
    {
        #region 1st way
        public static IServiceCollection AutoDatabaseMigration(this IServiceCollection services, IConfiguration configuration)
        {
            var tenantSettings = configuration.GetSection("TenantSettings").Get<TenantSettings>(); ;

            if (tenantSettings == null)
            {
                throw new ArgumentException("ConnectionString and Provider are required for each tenant.");
            }

            var provider = "MSSQL";
            string connectionString = "";
            if (tenantSettings.Defaults.Name == "default" && !string.IsNullOrEmpty(tenantSettings.Defaults.ConnectionString))
            {
                connectionString = tenantSettings.Defaults.ConnectionString;
                provider = tenantSettings.Defaults.DbProvider == null ? "MSSQL" : tenantSettings.Defaults.DbProvider;
                if (!connectionString.IsNullOrEmpty()) MigrationExecute(services, connectionString, provider);
            }

            if (tenantSettings.Tenants.Count > 0)
            {
                foreach (var tenant in tenantSettings.Tenants)
                {

                    provider = tenant.DbProvider.IsNullOrEmpty() ? "MSSQL" : tenant.DbProvider;
                    connectionString = tenant.ConnectionString;
                    if (!connectionString.IsNullOrEmpty()) MigrationExecute(services, connectionString, provider);

                }
            }


            return services;
        }

        private static void MigrationExecute(IServiceCollection services, string connectionString, string provider)
        {
            try
            {
                var migrationAssembly = typeof(RepositoryDBContext).Assembly.GetName().Name.IsNullOrEmpty() ? "PrettyAPI" : typeof(RepositoryDBContext).Assembly.GetName().Name;

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
            catch
            {
            }


        }

        #endregion

        #region 2nd way



        #endregion


    }

}
