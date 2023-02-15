using Microsoft.EntityFrameworkCore;
using PointOfSale.Api;
using PointOfSale.Infrastructure.Config;
using PointOfSale.Infrastructure.Context;
using Serilog;

const string CorsPolicyName = "AnyOriginAnyMethodAnyHeader";

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File("*/Logs"));

//Add configuration instances with options patterm
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<EventHubSettings>(builder.Configuration.GetSection("EventHubSettings"));
builder.Services.Configure<ExternalServiceSettings>(builder.Configuration.GetSection("EventHubSettings"));

builder.Services.AddDependencys();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SalesContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("AzureDb")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope
        .ServiceProvider
        .GetRequiredService<SalesContext>()
        .Database
        .EnsureCreated();
}

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Add global exception handler
app.UseMiddleware<GlobalExceptionHandler>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(CorsPolicyName);

app.Run();