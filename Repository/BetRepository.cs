using Dapper;
using SG_TechTest.Db;
using SG_TechTest.Models;

namespace SG_TechTest.Repository;

public interface IBetRepository
{
    Task<Bet?> GetById(Guid id);
    Task<Guid> Create(decimal amount, IEnumerable<(Guid FixtureId, int TeamId)> fixtures);
}

public class BetRepository : IBetRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public BetRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    
    public async Task<Bet?> GetById(Guid id)
    {
        var betQuery = $"SELECT * FROM Bets WHERE Id = '{id}'";
        var fixtureQuery = $"SELECT * FROM FixtureBets WHERE BetId = '{id}'";
        using var cn = await _connectionFactory.CreateConnection();
        var bet = await cn.QueryFirstOrDefaultAsync<Bet?>(betQuery);
        if (bet == null)
            return null;

        var fixtures = await cn.QueryAsync<FixtureBet>(fixtureQuery);
        bet.Fixtures = fixtures;
        
        return bet;
    }

    public async Task<Guid> Create(decimal amount, IEnumerable<(Guid FixtureId, int TeamId)> fixtures)
    {
        var newId = Guid.NewGuid();
        
        using var cn = await _connectionFactory.CreateConnection();
        using var transaction = cn.BeginTransaction();
        var betQuery = $"INSERT INTO Bets (Id, Amount) VALUES ('{newId}', '{amount}')";
        await cn.ExecuteAsync(betQuery);
        foreach (var fixture in fixtures)
        {
            var lookupQuery = $"INSERT INTO FixtureBets (FixtureId, BetId, TeamToWin) VALUES ('{fixture.FixtureId}', '{newId}', {fixture.TeamId})";
            await cn.ExecuteAsync(lookupQuery);
        }
        transaction.Commit();
        return newId;
    }
}