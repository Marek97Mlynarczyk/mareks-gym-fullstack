namespace MareksGym.Api.Application.Macros;

public sealed record MacroCalculationResult(
    double Bmr,
    double Tdee,
    double TargetCalories,
    double ProteinGrams,
    double CarbsGrams,
    double FatGrams
);
