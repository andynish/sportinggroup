namespace SG_TechTest.Models;

public class Fixture
{
    public Guid Id { get; set; } = Guid.Empty;
    public int TeamAId { get; set; }
    public string TeamA { get; set; } = default!;
    public int TeamBId { get; set; }
    public string TeamB { get; set; } = default!;
    public int WinnerId { get; set; }
    public string Winner { get; set; } = default!;
}