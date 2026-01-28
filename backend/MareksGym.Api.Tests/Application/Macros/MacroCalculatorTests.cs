using MareksGym.Api.Application.Macros;
using Xunit;

namespace MareksGym.Api.Tests.Application.Macros;

public class MacroCalculatorTests
{
    [Fact]
    public void Calculate_MaleModerateCut_ReturnsExpectedValues()
    {
        var calculator = new MacroCalculator();

        var input = new MacroCalculationInput(
            Sex: Sex.Male,
            Age: 29,
            HeightCm: 180,
            WeightKg: 76,
            ActivityLevel: ActivityLevel.Moderate,
            Goal: Goal.Cut
        );

        var result = calculator.Calculate(input);

        Assert.Equal(1745, result.Bmr);
        Assert.Equal(2705, result.Tdee);
        Assert.Equal(2205, result.TargetCalories);
        Assert.Equal(152, result.ProteinGrams);
    }
}
