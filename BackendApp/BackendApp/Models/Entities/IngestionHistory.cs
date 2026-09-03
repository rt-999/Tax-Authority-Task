namespace BonusSystem.Api.Models.Entities;

public class IngestionHistory
{
    public int Id { get; set; }
    public int MeasureId { get; set; }
    public int Year { get; set; }
    public string Period { get; set; } = string.Empty; // למשל: "רבעון 1"
    public string FileName { get; set; } = string.Empty;
    public DateTime IngestedAt { get; set; } = DateTime.UtcNow;
    public int RecordCount { get; set; }

    public Measure? Measure { get; set; }
    public ICollection<IngestedDataRow> DataRows { get; set; } = new List<IngestedDataRow>();
}