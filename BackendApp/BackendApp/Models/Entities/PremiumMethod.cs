namespace BonusSystem.Api.Models.Entities;

public class PremiumMethod
{
    public int Id { get; set; }
    public string MethodNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal PremiumPercentage { get; set; }
    public string CalculationPeriod { get; set; } = string.Empty; // למשל: חודשי, רבעוני

    public ICollection<Measure> Measures { get; set; } = new List<Measure>();
}