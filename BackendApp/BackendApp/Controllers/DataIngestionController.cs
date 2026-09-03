using Microsoft.AspNetCore.Mvc;
using BonusSystem.Api.Models.Dtos;
using BonusSystem.Api.Services.Interfaces;

namespace BonusSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataIngestionController : ControllerBase
{
    private readonly IIngestionService _ingestionService;

    public DataIngestionController(IIngestionService ingestionService)
    {
        _ingestionService = ingestionService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadExcel([FromForm] UploadFileDto dto)
    {
        if (dto.File == null || dto.File.Length == 0)
            return BadRequest("קובץ לא תקין");

        var result = await _ingestionService.ProcessExcelUploadAsync(dto);
        if (!result) return BadRequest("נכשלה קליטת הקובץ");

        return Ok(new { Message = "הקובץ נקלט בהצלחה" });
    }

    [HttpGet("data")]
    public async Task<IActionResult> GetIngestedData(
        [FromQuery] int measureId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var data = await _ingestionService.GetIngestedDataAsync(measureId, page, pageSize, search);
        return Ok(data);
    }
}