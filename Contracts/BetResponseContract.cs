using SG_TechTest.Models;

namespace SG_TechTest.Contracts;

public class BetResponseContract
{
    public Guid Id { get; set; }
    public bool Winner { get; set; }
    public IEnumerable<FixtureBet> Fixtures { get; set; } = new List<FixtureBet>();
}