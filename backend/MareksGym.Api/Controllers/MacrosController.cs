using MareksGym.Api.Application.Macros;
using MareksGym.Api.Contracts.Macros;
using Microsoft.AspNetCore.Mvc;

namespace MareksGym.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MacrosController : ControllerBase
{
    private readonly MacroCalculator _calculator;

    public MacrosController(MacroCalculator calculator)
    {
        _calculator = calculator;
    }

    [HttpPost("calculate")]
    public ActionResult<CalculateMacrosResponse> Calculate([FromBody] CalculateMacrosRequest request)
    {
        if (!TryMap(request, out var input, out var error))
            return BadRequest(new { error });

        var result = _calculator.Calculate(input);

        var response = new CalculateMacrosResponse(
            Bmr: result.Bmr,
            Tdee: result.Tdee,
            TargetCalories: result.TargetCalories,
            ProteinGrams: result.ProteinGrams,
            CarbsGrams: result.CarbsGrams,
            FatGrams: result.FatGrams
        );

        return Ok(response);
    }

    private static bool TryMap(
        CalculateMacrosRequest request,
        out MacroCalculationInput input,
        out string error)
    {
        error = string.Empty;

        if (!TryParseSex(request.Sex, out var sex))
        {
            input = default!;
            error = "Invalid sex. Use: male or female.";
            return false;
        }

        if (!TryParseActivity(request.ActivityLevel, out var activity))
        {
            input = default!;
            error = "Invalid activityLevel. Use: sedentary, light, moderate, active, very_active.";
            return false;
        }

        if (!TryParseGoal(request.Goal, out var goal))
        {
            input = default!;
            error = "Invalid goal. Use: cut, maintain, bulk.";
            return false;
        }

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
