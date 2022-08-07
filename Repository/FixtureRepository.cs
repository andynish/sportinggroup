using Dapper;
using SG_TechTest.Db;
using SG_TechTest.Models;

namespace SG_TechTest.Repository;

public interface IFixtureRepository
{
    Task<IEnumerable<Fixture>> GetAll();
    Task<Fixture?> GetById(Guid id);
    Task<Guid> Create(int teamA, int teamB);
    Task<Fixture?> Update(Guid fixtureId, int winner);
}

public class FixtureRepository : IFixtureRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public FixtureRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Fixture>> GetAll()
    {
        var query = @"SELECT f.Id, t1.Id AS TeamAId, t1.Name AS TeamA, t2.Id AS TeamBId, t2.Name AS TeamB, t3.Id AS WinnerId, t3.Name AS Winner FROM Fixtures f ";
        query += "INNER JOIN Teams t1 ON f.TeamA = t1.Id ";
        query += "INNER JOIN Teams t2 On f.TeamB = t2.Id ";
        query += "LEFT JOIN Teams t3 On f.Winner = t3.Id";
        using var cn = await _connectionFactory.CreateConnection();
        var fixtures = await cn.QueryAsync<Fixture>(query);
        return fixtures;
    }
    
    public async Task<Fixture?> GetById(Guid id)
    {
        var query = @"SELECT f.Id, t1.Id AS TeamAId, t1.Name AS TeamA, t2.Id AS TeamBId, t2.Name AS TeamB, t3.Id AS WinnerId, t3.Name AS Winner FROM Fixtures f ";
        query += "INNER JOIN Teams t1 ON f.TeamA = t1.Id ";
        query += "INNER JOIN Teams t2 On f.TeamB = t2.Id ";
        query += "LEFT JOIN Teams t3 On f.Winner = t3.Id ";
        query += $"WHERE f.Id = '{id}'";
        
        using var cn = await _connectionFactory.CreateConnection();
        var result = await cn.QueryFirstOrDefaultAsync<Fixture>(query);
        return result;
    }

    public async Task<Guid> Create(int teamA, int teamB)
    {
        var newId = Guid.NewGuid();
        var query = $"INSERT INTO Fixtures (Id, TeamA, TeamB) VALUES ('{newId}', '{teamA}', '{teamB}')";
        using var cn = await _connectionFactory.CreateConnection();
        var result = await cn.ExecuteAsync(query);
        return newId;
    }

    public async Task<Fixture?> Update(Guid fixtureId, int winner)
    {
        var query = $"UPDATE Fixtures SET Winner = {winner} WHERE Id = '{fixtureId}'";
        using var cn = await _connectionFactory.CreateConnection();
        await cn.ExecuteAsync(query);

        return await GetById(fixtureId);
    }
}