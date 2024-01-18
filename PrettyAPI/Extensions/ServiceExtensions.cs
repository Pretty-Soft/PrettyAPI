using Contracts;
using Entities;
using LoggerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NLog;
using Repository;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PrettyAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("PrettyCorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod().AllowAnyHeader());
            });
        }

        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options => { });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["ConnectionStrings:DefaultConnection"];

            services.AddDbContext<RepositoryDBContext>(options =>
            options.UseSqlServer(connectionString, b => b.MigrationsAssembly("PrettyAPI")));
           // services.AddDatabaseDeveloperPageExceptionFilter();

        }
        //public static void ConfigureMySqlContext(this IServiceCollection services, IConfiguration config)
        //{
        //    var connectionString = config["MySqlConnection:DefaultConnection"];
        //    services.AddDbContext<RepositoryContext>(o => o.UseMySql(connectionString,
        //        MySqlServerVersion.LatestSupportedServerVersion));
        //}

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }
        public static void ConfigureTokenRepositoryWrapper(this IServiceCollection services)
        {
            services.AddTransient<ITokenService, TokenService>();
        }
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration config)
        {
            var jwtSection = config.GetSection("JWT");  

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidAudience = jwtSection.GetValue<string>("ValidAudience"),
                        ValidIssuer = jwtSection.GetValue<string>("ValidIssuer"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection.GetValue<string>("Secret")))
                    };
                    options.IncludeErrorDetails = true;
                });
        }
    }
}
