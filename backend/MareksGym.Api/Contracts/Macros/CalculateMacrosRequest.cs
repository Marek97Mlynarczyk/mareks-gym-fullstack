namespace MareksGym.Api.Contracts.Macros;

public sealed record CalculateMacrosRequest(
    string Sex,     // "male" | "female"
    int Age,
    double HeightCm,
    double WeightKg,
    string ActivityLevel,   // "sedentary" | "light" | "moderate" | "active" | "very_active"
    string Goal     // "cut" | "maintain" | "bulk"
);
