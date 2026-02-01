namespace MareksGym.Api.Application.Macros;

public sealed record MacroCalculationInput(
    Sex Sex,
    int Age,
    int HeightCm,
    double WeightKg,
    ActivityLevel ActivityLevel,
    Goal Goal
);
