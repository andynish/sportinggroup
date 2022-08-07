using EnsureThat;
using SG_TechTest.Models;
using SG_TechTest.Repository;

namespace SG_TechTest.Services;

public interface IFixtureService
{
    Task<IEnumerable<Fixture>> GetAll();
    Task<Fixture?> GetById(Guid id);
    Task<Guid> Create(int teamA, int teamB);
    Task<Fixture?> SetWinner(Guid fixtureId, int winner);
}

public class FixtureService : IFixtureService
{
    private readonly IFixtureRepository _fixtureRepository;

    public FixtureService(IFixtureRepository fixtureRepository)
    {
        Ensure.Any.IsNotNull(fixtureRepository, nameof(fixtureRepository));
        _fixtureRepository = fixtureRepository;
    }
    
    public Task<IEnumerable<Fixture>> GetAll()
    {
        return _fixtureRepository.GetAll();
    }

    public Task<Fixture?> GetById(Guid id)
    {
        Ensure.Guid.IsNotEmpty(id, nameof(id));
        return _fixtureRepository.GetById(id);
    }

    public Task<Guid> Create(int teamA, int teamB)
    {
        Ensure.Any.IsNotDefault(teamA, nameof(teamA));
        Ensure.Any.IsNotDefault(teamB, nameof(teamB));
        
        return _fixtureRepository.Create(teamA, teamB);
    }

    public async Task<Fixture?> SetWinner(Guid fixtureId, int winner)
    {
        Ensure.Guid.IsNotEmpty(fixtureId, nameof(fixtureId));
        Ensure.Any.IsNotDefault(winner, nameof(winner));
        
        var fixture = await _fixtureRepository.GetById(fixtureId);
        
        if (fixture == null)
            throw new ArgumentException($"Invalid fixture: {fixtureId}");
        
        if (fixture.TeamAId != winner && fixture.TeamBId != winner)
            throw new ArgumentException($"Cannot set winner to {winner} as the team was not a participant.");
        
        return await _fixtureRepository.Update(fixtureId, winner);
    }
}