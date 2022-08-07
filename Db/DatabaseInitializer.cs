using Dapper;

namespace SG_TechTest.Db;

public class DatabaseInitializer
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DatabaseInitializer(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _connectionFactory.CreateConnection();
        using var transaction = connection.BeginTransaction();
        
        await connection.ExecuteAsync(@"CREATE TABLE IF NOT EXISTS Fixtures (
        Id CHAR(36) PRIMARY KEY, 
        TeamA INTEGER NOT NULL,
        TeamB INTEGER NOT NULL,
        Winner INTEGER NULL)");
        
        await connection.ExecuteAsync(@"CREATE TABLE IF NOT EXISTS Bets (
        Id CHAR(36) PRIMARY KEY, 
        Amount NUMERIC NOT NULL)");
        
        await connection.ExecuteAsync(@"CREATE TABLE IF NOT EXISTS FixtureBets (
        FixtureId CHAR(36) PRIMARY KEY, 
        BetId CHAR(36) NOT NULL,
        TeamToWin INTEGER NOT NULL)");
        
        await connection.ExecuteAsync(@"CREATE TABLE IF NOT EXISTS Teams (
        Id INTEGER PRIMARY KEY, 
        Name TEXT NOT NULL)");

        var teamCount = await connection.QueryFirstAsync<int>("SELECT COUNT(*) FROM Teams");
        if(teamCount == 0 )
        {
            await connection.ExecuteAsync(@"INSERT INTO Teams (Id, Name) VALUES (1, 'Liverpool')");
            await connection.ExecuteAsync(@"INSERT INTO Teams (Id, Name) VALUES (2, 'Man City')");
            await connection.ExecuteAsync(@"INSERT INTO Teams (Id, Name) VALUES (3, 'Arsenal')");
            await connection.ExecuteAsync(@"INSERT INTO Teams (Id, Name) VALUES (4, 'Chelsea')");
            await connection.ExecuteAsync(@"INSERT INTO Teams (Id, Name) VALUES (5, 'Man United')");
            await connection.ExecuteAsync(@"INSERT INTO Teams (Id, Name) VALUES (6, 'Tottenham')");
            await connection.ExecuteAsync(@"INSERT INTO Teams (Id, Name) VALUES (7, 'Aston Villa')");
            await connection.ExecuteAsync(@"INSERT INTO Teams (Id, Name) VALUES (8, 'Everton')");
            await connection.ExecuteAsync(@"INSERT INTO Teams (Id, Name) VALUES (9, 'Crystal Palace')");
            await connection.ExecuteAsync(@"INSERT INTO Teams (Id, Name) VALUES (10, 'Leicester City')");
        }

        transaction.Commit();
    }
}