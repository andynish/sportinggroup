using System.Diagnostics.Contracts;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using SG_TechTest;
using SG_TechTest.Contracts;
using SG_TechTest.Db;
using SG_TechTest.Models;
using SG_TechTest.Repository;
using SG_TechTest.Services;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
    new DbConnectionFactory.SqliteConnectionFactory(config.GetValue<string>("Database:ConnectionString")));
builder.Services.AddSingleton<DatabaseInitializer>();

SqlMapper.AddTypeHandler(new GuidTypeMapper());
SqlMapper.RemoveTypeMap(typeof(Guid));
SqlMapper.RemoveTypeMap(typeof(Guid?));

builder.Services.AddTransient<IFixtureService, FixtureService>();
builder.Services.AddTransient<IFixtureRepository, FixtureRepository>();

builder.Services.AddTransient<IBettingService, BettingService>();
builder.Services.AddTransient<IBetRepository, BetRepository>();

var app = builder.Build();

EndpointMapper.MapEndpoints(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await databaseInitializer.InitializeAsync();

app.Run();