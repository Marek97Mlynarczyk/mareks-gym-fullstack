namespace MareksGym.Api.Contracts.Macros.History;

public sealed record MacroHistoryItem(
    int Id,
    string Sex,
    int Age,
    int HeightCm,
    double WeightKg,
    string ActivityLevel,
    string Goal,
    double Bmr,
    double Tdee,
    double TargetCalories,
    double ProteinGrams,
    double CarbsGrams,
    double FatGrams,
    DateTime CreatedAtUtc
);

public sealed record GetMacroHistoryResponse(
    int Page,
    int PageSize,
    int TotalCount,
    IReadOnlyList<MacroHistoryItem> Items
);
