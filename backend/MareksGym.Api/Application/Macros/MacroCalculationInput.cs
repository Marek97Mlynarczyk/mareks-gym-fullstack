namespace MareksGym.Api.Application.Macros;

public sealed record MacroCalculationInput(
    Sex Sex,
    int Age,
    double HeightCm,
    double WeightKg,
    ActivityLevel ActivityLevel,
    Goal Goal
);
