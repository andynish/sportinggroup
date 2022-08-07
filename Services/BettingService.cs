using EnsureThat;
using SG_TechTest.Models;
using SG_TechTest.Repository;

namespace SG_TechTest.Services;

public interface IBettingService
{
    Task<Bet?> GetById(Guid id);
    Task<Guid> PlaceBet(decimal amount, IEnumerable<(Guid FixtureId, int TeamId)> fixtures);
}

public class BettingService : IBettingService
{
    private readonly IBetRepository _betRepository;
    private readonly IFixtureRepository _fixtureRepository;

    public BettingService(IBetRepository betRepository, IFixtureRepository fixtureRepository)
    {
        Ensure.Any.IsNotNull(betRepository, nameof(betRepository));
        Ensure.Any.IsNotNull(fixtureRepository, nameof(fixtureRepository));
        
        _betRepository = betRepository;
        _fixtureRepository = fixtureRepository;
    }
    
    public Task<Bet?> GetById(Guid id)
    {
        Ensure.Guid.IsNotEmpty(id, nameof(id));
        return _betRepository.GetById(id);
    }

    public async Task<Guid> PlaceBet(decimal amount, IEnumerable<(Guid FixtureId, int TeamId)> fixtures)
    {
        var fixtureList = fixtures.ToList();
        foreach (var fixture in fixtureList)
        {
            Ensure.Guid.IsNotEmpty(fixture.FixtureId, nameof(fixture.FixtureId));
            Ensure.Any.IsNotDefault(fixture.TeamId, nameof(fixture.TeamId));
            
            var currentFixture = await _fixtureRepository.GetById(fixture.FixtureId);
            if (currentFixture == null)
                throw new ArgumentException($"Invalid fixture: {fixture.FixtureId}");
            
            if(currentFixture.TeamAId != fixture.TeamId && currentFixture.TeamBId != fixture.TeamId)
                throw new ArgumentException($"Cannot set TeamToWin to {fixture.TeamId} as the team was not a participant.");
        }
            
        return await _betRepository.Create(amount, fixtureList);
    }
}