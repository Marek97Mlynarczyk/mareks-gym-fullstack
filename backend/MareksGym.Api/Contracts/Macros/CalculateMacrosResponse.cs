namespace MareksGym.Api.Contracts.Macros;

public sealed record CalculateMacrosResponse(
    double Bmr,
    double Tdee,
    double TargetCalories,
    double ProteinGrams,
    double CarbsGrams,
    double FatGrams
);
