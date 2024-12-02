using Contracts;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace PrettyAPI.Extensions
{
    public static class DatabaseInitializer
    {
        public static void EnsureDatabaseMigrated(string connectionString, string provider, ITenantService tenantService)
        {
            try
            {
                var migrationAssembly = typeof(RepositoryDBContext).Assembly.GetName().Name.IsNullOrEmpty() ? "PrettyAPI" : typeof(RepositoryDBContext).Assembly.GetName().Name;

                var optionsBuilder = new DbContextOptionsBuilder<RepositoryDBContext>();

                // Add conditions for other database providers if needed
                if (provider.Equals("MSSQL", StringComparison.OrdinalIgnoreCase))
                {
                    //optionsBuilder.UseSqlServer(connectionString);
                    optionsBuilder.UseSqlServer(connectionString, builder =>
                    {
                        builder.MigrationsAssembly(migrationAssembly);
                    });
                }
                else if (provider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase))
                {
                   // optionsBuilder.UseNpgsql(connectionString);
                    optionsBuilder.UseNpgsql(connectionString, builder =>
                    {
                        builder.MigrationsAssembly(migrationAssembly);
                    });
                }
                else if (provider.Equals("MySQL", StringComparison.OrdinalIgnoreCase))
                {
                    //optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 2))); // Adjust version as needed
                    optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 2)), builder =>
                    {
                        builder.MigrationsAssembly(migrationAssembly);
                    });
                }
                else if (provider.Equals("Oracle", StringComparison.OrdinalIgnoreCase))
                {
                    //optionsBuilder.UseOracle(connectionString);
                    optionsBuilder.UseOracle(connectionString, builder =>
                    {
                        builder.MigrationsAssembly(migrationAssembly);
                    });
                }
                else
                {
                    throw new ArgumentException($"Unsupported database provider: {provider}");
                }

                using (var context = new RepositoryDBContext(optionsBuilder.Options, tenantService))
                {
                    if (!context.Database.CanConnect())
                    {
                        context.Database.EnsureCreated(); // Ensure DB is created if missing
                        context.Database.Migrate();       // Apply any pending migrations
                    }
                }
            }
            catch (Exception ex)
            {
                throw; // Rethrow to prevent login continuation if critical
            }
        }
    }

}
