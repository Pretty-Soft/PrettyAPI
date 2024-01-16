using Contracts;
using Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using NLog;
using PrettyAPI.CustomMiddleware;
using PrettyAPI.Extensions;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//log file directory
LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/NLog/nlog.config"));
// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureSqlContext(builder.Configuration);
//builder.Services.ConfigureMySqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryWrapper();
builder.Services.AddAutoMapper(typeof(Program));

//jwt
builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:44310",
            ValidAudience = "https://localhost:44310",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JWTsjjfjhicodehjhdu44rfddTonjhgyufrsenHIGHsecuredPasswordVVVp1OH7Xzyr"))
        };
    });

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
