using MareksGym.Api.Application.Common;
using MareksGym.Api.Contracts.Macros;

namespace MareksGym.Api.Application.Macros;

// This class is responsible for validating incoming API requests and mapping them into a strongly-typed input for the calculator
// Keeping this logic out of the controller keeps HTTP code simple and makes validation easier to test and change later
public sealed class MacroRequestMapper
{
    public bool TryMap(
        CalculateMacrosRequest request,
        out MacroCalculationInput input,
        out ValidationResult validation)
    {
        validation = new ValidationResult();

        // Enum-like fields come in as strings from the API. I validate and parse them explicitly to control error messages and avoid throwing exceptions for user input mistakes
        if (!TryParseSex(request.Sex, out var sex))
            validation.AddError("sex", "Invalid value. Allowed: male, female.");

        if (!TryParseActivity(request.ActivityLevel, out var activity))
            validation.AddError(
                "activityLevel",
                "Invalid value. Allowed: sedentary, light, moderate, active, very_active.");

        if (!TryParseGoal(request.Goal, out var goal))
            validation.AddError("goal", "Invalid value. Allowed: cut, maintain, bulk.");

        if (request.Age < 13 || request.Age > 120)
            validation.AddError("age", "Must be between 13 and 120.");

        if (request.HeightCm < 120 || request.HeightCm > 230)
            validation.AddError("heightCm", "Must be between 120 and 230.");

        if (request.WeightKg < 30 || request.WeightKg > 300)
            validation.AddError("weightKg", "Must be between 30 and 300.");

        // If any validation errors were added, stop here and return all errors at once
        if (!validation.IsValid)
        {
            input = default!;
            return false;
        }

        // If request is valid, build the calculation input
        input = new MacroCalculationInput(
            Sex: sex,
            Age: request.Age,
            HeightCm: request.HeightCm,
            WeightKg: request.WeightKg,
            ActivityLevel: activity,
            Goal: goal
        );

        return true;
    }

    private static bool TryParseSex(string value, out Sex sex)
    {
        sex = default;
        if (string.IsNullOrWhiteSpace(value)) return false;

        return value.Trim().ToLowerInvariant() switch
        {
            "male" => (sex = Sex.Male) == Sex.Male,
            "female" => (sex = Sex.Female) == Sex.Female,
            _ => false
        };
    }

    private static bool TryParseActivity(string value, out ActivityLevel activity)
    {
        activity = default;
        if (string.IsNullOrWhiteSpace(value)) return false;

        return value.Trim().ToLowerInvariant() switch
        {
            "sedentary" => (activity = ActivityLevel.Sedentary) == ActivityLevel.Sedentary,
            "light" => (activity = ActivityLevel.Light) == ActivityLevel.Light,
            "moderate" => (activity = ActivityLevel.Moderate) == ActivityLevel.Moderate,
            "active" => (activity = ActivityLevel.Active) == ActivityLevel.Active,
            "very_active" => (activity = ActivityLevel.VeryActive) == ActivityLevel.VeryActive,
            "veryactive" => (activity = ActivityLevel.VeryActive) == ActivityLevel.VeryActive,
            _ => false
        };
    }

    private static bool TryParseGoal(string value, out Goal goal)
    {
        goal = default;
        if (string.IsNullOrWhiteSpace(value)) return false;

        return value.Trim().ToLowerInvariant() switch
        {
            "cut" => (goal = Goal.Cut) == Goal.Cut,
            "maintain" => (goal = Goal.Maintain) == Goal.Maintain,
            "bulk" => (goal = Goal.Bulk) == Goal.Bulk,
            _ => false
        };
    }
}