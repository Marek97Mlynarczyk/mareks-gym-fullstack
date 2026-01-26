namespace MareksGym.Api.Application.Macros;

public sealed class MacroCalculator
{
    public MacroCalculationResult Calculate(MacroCalculationInput input)
    {
        // BMR calculation (using Mifflin–St Jeor formula)
        double bmr;

        if (input.Sex == Sex.Male)
        {
            bmr = 10 * input.WeightKg + 6.25 * input.HeightCm - 5 * input.Age + 5;
        }
        else
        {
            bmr = 10 * input.WeightKg + 6.25 * input.HeightCm - 5 * input.Age - 161;
        }

        // Applying activity multiplier to estimate Total Daily Energy Expenditure (TDEE)
        var activityMultiplier = input.ActivityLevel switch
        {
            ActivityLevel.Sedentary => 1.2,
            ActivityLevel.Light => 1.375,
            ActivityLevel.Moderate => 1.55,
            ActivityLevel.Active => 1.725,
            ActivityLevel.VeryActive => 1.9,
            _ => throw new ArgumentOutOfRangeException()
        };

        var tdee = bmr * activityMultiplier;

        // Adjusting calories based on goal (cut / maintain / bulk)
        var targetCalories = input.Goal switch
        {
            Goal.Cut => tdee - 500,
            Goal.Maintain => tdee,
            Goal.Bulk => tdee + 300,
            _ => throw new ArgumentOutOfRangeException()
        };

        //  Calculating macronutrient split. Macro defaults I'm using for strength and hypertrophy training: Protein: 2.0g/kg, Fat: 25% calories, Carbs: remaining calories
        var proteinGrams = input.WeightKg * 2.0;

        var fatCalories = targetCalories * 0.25;
        var fatGrams = fatCalories / 9.0;

        var remainingCalories = targetCalories - (proteinGrams * 4.0) - fatCalories;
        var carbsGrams = remainingCalories / 4.0;

        return new MacroCalculationResult(
            Bmr: Math.Round(bmr, 0),
            Tdee: Math.Round(tdee, 0),
            TargetCalories: Math.Round(targetCalories, 0),
            ProteinGrams: Math.Round(proteinGrams, 0),
            CarbsGrams: Math.Round(carbsGrams, 0),
            FatGrams: Math.Round(fatGrams, 0)
        );
    }
}
