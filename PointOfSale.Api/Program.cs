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

//Sets all dependencys
builder.Services.AddDependencys(builder.Configuration);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Setup endpoints
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

if(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "integrationtest") 
{ 
    builder.Services.AddSwaggerGen();
}
else
{
    builder.Services.AddSwaggerGen(o => o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "PointOfSaleApi.xml")));
}

//Setup db
builder.Services.AddDbContext<SalesContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("AzureDb")));

var app = builder.Build();

//Validate db
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

//Visibility for Integration tests
public partial class Program { }