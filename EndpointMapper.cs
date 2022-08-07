using Microsoft.AspNetCore.Mvc;
using SG_TechTest.Contracts;
using SG_TechTest.Services;

namespace SG_TechTest;

public static class EndpointMapper
{
    public static void MapEndpoints(WebApplication app)
    {
        app.MapGet("/api/fixtures", async (IFixtureService service) => await service.GetAll());
        app.MapGet("/api/fixtures/{id}", async (Guid id, IFixtureService service) =>
        {
            var result = await service.GetById(id);
            return result == null ? Results.NotFound() : Results.Ok(result);
        });
        app.MapGet("/api/bets/{id}", async (Guid id, IBettingService service) =>
        {
            var bet = await service.GetById(id);
            if (bet == null)
                return Results.NotFound();

            var result = new BetResponseContract
            {
                Id = bet.Id,
                Fixtures = bet.Fixtures,
                Winner = Random.Shared.Next(1, 100) % 2 == 0
            };
            
            return Results.Ok(result);
        });
        
        
        app.MapPost("/api/fixtures", async ([FromBody] FixturePostRequestContract contract, IFixtureService service) => 
            await service.Create(contract.TeamA, contract.TeamB));
        app.MapPut("/api/fixtures", async ([FromBody] FixturePutRequestContract contract, IFixtureService service) =>
        {
            var result = await service.SetWinner(contract.FixtureId, contract.Winner);
            return result == null ? Results.NotFound() : Results.Ok(result);
        });
        app.MapPost("/api/bets", async ([FromBody] BetRequestContract contract, IBettingService service) =>
        {
            var fixtures = contract.FixturesWithWinner.Select(x => (x.FixtureId, x.TeamToWin));
            return await service.PlaceBet(contract.Amount, fixtures);
        });
    }
}