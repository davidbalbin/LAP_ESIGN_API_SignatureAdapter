using LapDrive.SignatureAdapter.API.Middleware;
using LapDrive.SignatureAdapter.API.Extensions;
using LapDrive.SignatureAdapter.API.Configuration;
using LapDrive.SignatureAdapter.Business.Extensions;
using LapDrive.SignatureAdapter.Data.Extensions;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

// Add services to the container
builder.Services.AddControllers();

// Configure API versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var apiSettings = builder.Configuration.GetSection("Api").Get<ApiSettings>()
        ?? new ApiSettings();
            
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = apiSettings.Title,
        Version = apiSettings.Version,
        Description = apiSettings.Description,
        Contact = new OpenApiContact
        {
            Name = apiSettings.Contact.Name,
            Email = apiSettings.Contact.Email
        }
    });

    // Add XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    options.EnableAnnotations();
});

// Register services
builder.Services.AddApiServices();
builder.Services.AddBusinessServices();
builder.Services.AddDataServices(builder.Configuration);

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder => builder
            .WithOrigins("https://yourdomain.com")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "LapDrive Signature Adapter API v1");
    });
}

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();

// Global exception handling
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("AllowSpecificOrigins");
app.UseAuthorization();

app.MapControllers();

app.Run();