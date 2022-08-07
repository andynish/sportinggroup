namespace SG_TechTest.Models;

public class FixtureBet
{
    public Guid FixtureId { get; set; }
    public Guid BetId { get; set; }
    public int TeamToWin { get; set; }
}