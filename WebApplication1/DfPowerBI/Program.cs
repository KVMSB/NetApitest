using AutoMapper;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services;
using Services.Interface;
using Services.Mapper;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


// Add controllers
builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = "https://login.microsoftonline.com/" + configuration.GetValue<string>("TenantId")+"/v2.0";
            options.Audience = configuration.GetValue<string>("ClientId");
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "https://login.microsoftonline.com/d022fc40-e3d0-4b60-af21-740a92219103/v2.0",
                ValidateAudience = true,
                ValidAudience = "bd8ff232-34b4-4999-8796-4700e5b30a88",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // Optional: Adjust for clock skew
            };
            options.MetadataAddress = $"https://login.microsoftonline.com/{configuration.GetValue<string>("TenantId")}/v2.0/.well-known/openid-configuration";
        });

builder.Services.AddAuthorization();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin() // Allow all origins
              .AllowAnyHeader() // Allow all headers
              .AllowAnyMethod(); // Allow all methods
    });
});

// Add AutoMapper
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<MapperProfile>(); // Add your profile(s) here
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// Configure DbContext
builder.Services.AddDbContext<DfpowerbiDevContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectionString")));

// Register repositories and services
builder.Services.AddScoped<IReportRepo, ReportRepo>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IPowerBIEmbedService, PowerBIEmbedService>();

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DFPowerBi", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and your token."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddLogging(options =>
{
    options.AddConsole();
    options.AddDebug();
});
var app = builder.Build();

// Apply CORS middleware first
app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
