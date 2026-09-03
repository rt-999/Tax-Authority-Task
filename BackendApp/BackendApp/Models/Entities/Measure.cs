namespace BonusSystem.Api.Models.Entities;

public class Measure
{
    public int Id { get; set; }
    public int PremiumMethodId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string SourceType { get; set; } = string.Empty; // קובץ Excel / ממשק
    public string SourceName { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;

    public PremiumMethod? PremiumMethod { get; set; }
    public ICollection<IngestionHistory> IngestionHistories { get; set; } = new List<IngestionHistory>();
}