namespace SG_TechTest.Models;

public class Bet
{
    public Guid Id { get; set; } = Guid.Empty;
    public IEnumerable<FixtureBet> Fixtures { get; set; } = new List<FixtureBet>();
}