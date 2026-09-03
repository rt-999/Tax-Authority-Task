namespace BonusSystem.Api.Models.Dtos;

public class UploadFileDto
{
    public int MeasureId { get; set; }
    public int Year { get; set; }
    public string Period { get; set; } = string.Empty;
    public IFormFile File { get; set; } = null!;
}