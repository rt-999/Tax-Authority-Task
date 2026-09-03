using BonusSystem.Api.Models.Dtos;

namespace BonusSystem.Api.Services.Interfaces;

public interface IIngestionService
{
    Task<bool> ProcessExcelUploadAsync(UploadFileDto dto);
    Task<DynamicDataResponseDto> GetIngestedDataAsync(int measureId, int page, int pageSize, string? search);
}