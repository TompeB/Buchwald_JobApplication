using Microsoft.EntityFrameworkCore;
using PointOfSale.Api;
using PointOfSale.Infrastructure.Config;
using PointOfSale.Infrastructure.Context;
using PointOfSale.Infrastructure.Repositories;
using PointOfSale.Shared.Interfaces.DataAccess;
using Serilog;
using System.Configuration;

const string CorsPolicyName = "AnyOriginAnyMethodAnyHeader";

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File("*/Logs"));

//Add configuration instances with options patterm
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<EventHubSettings>(builder.Configuration.GetSection("EventHubSettings"));

builder.Services.AddDependencys();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SalesContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("AzureDb")));

var app = builder.Build();

//Migrate database
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<SalesContext>();
    //dataContext.Database.Migrate();
    //dataContext.Database.EnsureCreated();
}

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(CorsPolicyName);

app.Run();