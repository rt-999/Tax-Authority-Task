namespace BonusSystem.Api.Models.Dtos;

public class DynamicDataResponseDto
{
    public List<string> Columns { get; set; } = new();
    public List<Dictionary<string, object?>> Rows { get; set; } = new();
    public int TotalCount { get; set; }
}