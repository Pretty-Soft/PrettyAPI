using Contracts;
using Entities.Models;
using Entities.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer     
{
    public class RepositoryDBContext : DbContext
    {
        public long TenantId { get; set; }
        private readonly ITenantService _tenantService;

        public RepositoryDBContext(DbContextOptions<RepositoryDBContext> options, ITenantService tenantService)
            : base(options)
        {
            _tenantService = tenantService;
        }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<LoginModel> LoginModels { get; set; }
        public DbSet<Tenant> Tenants { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //TenantId = _tenantService.GetTenant().VendorId;
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Owner>().HasQueryFilter(a => a.TenantId == 1);
            modelBuilder.Entity<Account>().HasQueryFilter(a => a.TenantId == 1);
            modelBuilder.Entity<LoginModel>().HasData(new LoginModel
            {
                Id = 1,
                UserName = "zs.islam93@gmail.com",
                Password = "Z@d123"
            });
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            var tenantConnectionString = _tenantService.GetConnectionString();
            var dbProvider = _tenantService.GetDatabaseProvider();
            var migrationAssembly = typeof(RepositoryDBContext).Assembly.GetName().Name.IsNullOrEmpty() ? "PrettyAPI" : typeof(RepositoryDBContext).Assembly.GetName().Name;

            if (!string.IsNullOrEmpty(tenantConnectionString))
            {
                if (dbProvider.Equals("MSSQL", StringComparison.OrdinalIgnoreCase))
                {
                    optionsBuilder.UseSqlServer(tenantConnectionString, builder =>
                    {
                        builder.MigrationsAssembly(migrationAssembly);
                    });
                }
                else if (dbProvider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase))
                {
                    optionsBuilder.UseNpgsql(tenantConnectionString, builder =>
                    {
                        builder.MigrationsAssembly(migrationAssembly);
                    });
                }
                else if (dbProvider.Equals("MySQL", StringComparison.OrdinalIgnoreCase))
                {
                    optionsBuilder.UseMySql(tenantConnectionString, new MySqlServerVersion(new Version(8, 0, 2)), builder =>
                    {
                        builder.MigrationsAssembly(migrationAssembly);
                    });
                }
                else if (dbProvider.Equals("Oracle", StringComparison.OrdinalIgnoreCase))
                {
                    optionsBuilder.UseOracle(tenantConnectionString, builder =>
                    {
                        builder.MigrationsAssembly(migrationAssembly);
                    });
                }
                else
                {
                    throw new ArgumentException($"Unsupported database provider: {dbProvider}");
                }
                // Add conditions for other database providers if needed
            }
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
          
            foreach (var entry in ChangeTracker.Entries<BaseEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Modified:
                        entry.Entity.TenantId = TenantId;
                        break;
                }
            }
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }

    }
}
