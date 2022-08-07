namespace SG_TechTest.Contracts;

public class BetRequestContract
{
    public decimal Amount { get; set; }
    public IEnumerable<FixtureWithWinner> FixturesWithWinner { get; set; } = new List<FixtureWithWinner>();
}

public class FixtureWithWinner
{
    public Guid FixtureId { get; set; }
    public int TeamToWin { get; set; }
}