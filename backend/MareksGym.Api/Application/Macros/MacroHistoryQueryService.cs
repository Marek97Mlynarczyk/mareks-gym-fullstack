using MareksGym.Api.Contracts.Macros.History;
using MareksGym.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MareksGym.Api.Application.Macros;
public sealed class MacroHistoryQueryService
{
    private readonly ApplicationDbContext _db;

    public MacroHistoryQueryService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<GetMacroHistoryResponse> GetAsync(int page, int pageSize, CancellationToken ct)
    {
        // Avoiding accidental huge queries
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;
        if (pageSize > 100) pageSize = 100;

        var totalCount = await _db.MacroCalculationHistories.CountAsync(ct);

        var items = await _db.MacroCalculationHistories
            .AsNoTracking() // Don't track entities I'm not updating
            .OrderByDescending(x => x.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new MacroHistoryItem(
                x.Id,
                x.Sex,
                x.Age,
                x.HeightCm,
                x.WeightKg,
                x.ActivityLevel,
                x.Goal,
                x.Bmr,
                x.Tdee,
                x.TargetCalories,
                x.ProteinGrams,
                x.CarbsGrams,
                x.FatGrams,
                x.CreatedAtUtc
            ))
            .ToListAsync(ct);

        return new GetMacroHistoryResponse(
            Page: page,
            PageSize: pageSize,
            TotalCount: totalCount,
            Items: items
        );
    }
}
