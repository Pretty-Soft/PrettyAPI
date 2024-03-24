using Contracts;
using Entities;
using Entities.Tenants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NLog;
using PrettyAPI.CustomMiddleware;
using PrettyAPI.Extensions;
using Repository.Tenants;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//log file directory
LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/NLog/nlog.config"));

builder.Services.AddHttpContextAccessor();
// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureTenantService();
//database-per-tenant config
builder.Services.ConfigureTenantSettings(builder.Configuration);
builder.Services.AddDataProtection();
builder.Services.AutoDatabaseMigration(builder.Configuration);
builder.Services.ConfigureRepositoryWrapper();
builder.Services.ConfigureTokenRepositoryWrapper();
builder.Services.ConfigureJWT(builder.Configuration);


builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();


var app = builder.Build();

//exception handler middleware
app.ConfigureCustomExceptionMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseExceptionHandler("/Error"); app.UseHsts();


app.UseHttpsRedirection();

//enables using static files for the request.
//If we don’t set a path to the static files, it will use a wwwroot folder in our solution explorer by default.
app.UseStaticFiles();

//will forward proxy headers to the current request. This will help us during the Linux deployment.
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("PrettyCorsPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
