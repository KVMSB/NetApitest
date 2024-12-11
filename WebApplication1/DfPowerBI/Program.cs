using AutoMapper;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Services;
using Services.Interface;
using Services.Mapper;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


// Add controllers
builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));


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
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
