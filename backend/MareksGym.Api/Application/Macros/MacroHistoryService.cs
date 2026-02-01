using MareksGym.Api.Infrastructure.Persistence;
using MareksGym.Api.Infrastructure.Persistence.Entities;

namespace MareksGym.Api.Application.Macros;


public class MacroHistoryService
{
    private readonly ApplicationDbContext _db;

    public MacroHistoryService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task SaveAsync(
        MacroCalculationInput input,
        MacroCalculationResult result,
        CancellationToken ct = default)
    {
        var entity = new MacroCalculationHistory
        {
            Sex = input.Sex.ToString().ToLowerInvariant(),
            Age = input.Age,
            HeightCm = input.HeightCm,
            WeightKg = input.WeightKg,
            ActivityLevel = input.ActivityLevel.ToString().ToLowerInvariant(),
            Goal = input.Goal.ToString().ToLowerInvariant(),

            Bmr = result.Bmr,
            Tdee = result.Tdee,
            TargetCalories = result.TargetCalories,
            ProteinGrams = result.ProteinGrams,
            CarbsGrams = result.CarbsGrams,
            FatGrams = result.FatGrams,

            CreatedAtUtc = DateTime.UtcNow
        };

        _db.MacroCalculationHistories.Add(entity);
        await _db.SaveChangesAsync(ct);
    }
}
