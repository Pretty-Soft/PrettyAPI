using Contracts;
using DataLayer;
using Entities;
using Entities.Tenants;
using LoggerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NLog;
using Repository;
using Repository.Tenants;
using System;
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
        public static void ConfigureTenantService(this IServiceCollection services)
        {
            services.AddTransient<ITenantService, TenantService>();
        }
        public static void ConfigureTenantSettings(this IServiceCollection services, IConfiguration config)
        {
            //Configure tenant setting
            services.Configure<TenantSettings>(config.GetSection(nameof(TenantSettings)));
        }
        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            
        }
        public static void ConfigureTokenRepositoryWrapper(this IServiceCollection services)
        {
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IJwtBlacklistService, JwtBlacklistRepository> ();
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
