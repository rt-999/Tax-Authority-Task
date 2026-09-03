using System.Text.Json;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using BonusSystem.Api.Data;
using BonusSystem.Api.Models.Dtos;
using BonusSystem.Api.Models.Entities;
using BonusSystem.Api.Services.Interfaces;

namespace BonusSystem.Api.Services;

public class IngestionService : IIngestionService
{
    private readonly ApplicationDbContext _context;

    public IngestionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ProcessExcelUploadAsync(UploadFileDto dto)
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using var stream = dto.File.OpenReadStream();
        using var reader = ExcelReaderFactory.CreateReader(stream);

        var result = reader.AsDataSet(new ExcelDataSetConfiguration()
        {
            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
            {
                UseHeaderRow = true // השורה הראשונה בקובץ תשמש ככותרות עמודות
            }
        });

        var table = result.Tables[0];
        if (table == null || table.Rows.Count == 0) return false;

        // 1. יצירת רשומת היסטוריית קליטה
        var history = new IngestionHistory
        {
            MeasureId = dto.MeasureId,
            Year = dto.Year,
            Period = dto.Period,
            FileName = dto.File.FileName,
            RecordCount = table.Rows.Count,
            IngestedAt = DateTime.UtcNow
        };

        _context.IngestionHistories.Add(history);
        await _context.SaveChangesAsync();

        // 2. המרת כל שורה ב-Excel ל-Dictionary והפיכתה ל-JSON Payload
        var rowsToInsert = new List<IngestedDataRow>();

        foreach (System.Data.DataRow row in table.Rows)
        {
            var rowDict = new Dictionary<string, object?>();
            foreach (System.Data.DataColumn col in table.Columns)
            {
                var value = row[col];
                rowDict[col.ColumnName] = value == DBNull.Value ? null : value.ToString();
            }

            var jsonString = JsonSerializer.Serialize(rowDict);

            rowsToInsert.Add(new IngestedDataRow
            {
                IngestionHistoryId = history.Id,
                DataJson = jsonString
            });
        }

        // 3. שמירה מרוכזת בבסיס הנתונים
        await _context.IngestedDataRows.AddRangeAsync(rowsToInsert);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<DynamicDataResponseDto> GetIngestedDataAsync(int measureId, int page, int pageSize, string? search)
    {
        // מציאת קליטה אחרונה עבור המדד
        var latestHistory = await _context.IngestionHistories
            .Where(h => h.MeasureId == measureId)
            .OrderByDescending(h => h.IngestedAt)
            .FirstOrDefaultAsync();

        if (latestHistory == null)
            return new DynamicDataResponseDto();

        var query = _context.IngestedDataRows
            .Where(r => r.IngestionHistoryId == latestHistory.Id);

        var totalCount = await query.CountAsync();

        var rawRows = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => r.DataJson)
            .ToListAsync();

        var parsedRows = new List<Dictionary<string, object?>>();
        var columnsSet = new HashSet<string>();

        foreach (var json in rawRows)
        {
            var dict = JsonSerializer.Deserialize<Dictionary<string, object?>>(json);
            if (dict != null)
            {
                parsedRows.Add(dict);
                foreach (var key in dict.Keys)
                {
                    columnsSet.Add(key);
                }
            }
        }

        // סינון בסיסי בזיכרון (במידה ונשלח חיפוש)
        if (!string.IsNullOrWhiteSpace(search))
        {
            parsedRows = parsedRows
                .Where(row => row.Values.Any(val => val != null && val.ToString()!.Contains(search, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        return new DynamicDataResponseDto
        {
            Columns = columnsSet.ToList(),
            Rows = parsedRows,
            TotalCount = totalCount
        };
    }
}