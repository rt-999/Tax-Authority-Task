namespace BonusSystem.Api.Models.Entities;

public class IngestedDataRow
{
    public long Id { get; set; }
    public int IngestionHistoryId { get; set; }

    // עמודת ה-JSON המרכזית שמכילה את שורת הנתונים הדינמית
    public string DataJson { get; set; } = "{}";

    public IngestionHistory? IngestionHistory { get; set; }
}