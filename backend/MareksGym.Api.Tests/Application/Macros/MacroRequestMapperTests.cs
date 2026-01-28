using MareksGym.Api.Application.Macros;
using MareksGym.Api.Contracts.Macros;
using Xunit;

namespace MareksGym.Api.Tests.Application.Macros;

public class MacroRequestMapperTests
{
    [Fact]
    public void TryMap_InvalidRequest_ReturnsValidationErrors()
    {
        var mapper = new MacroRequestMapper();

        var request = new CalculateMacrosRequest(
            Sex: "invalid",
            Age: 5,
            HeightCm: 50,
            WeightKg: 10,
            ActivityLevel: "wrong",
            Goal: "nope"
        );

        var success = mapper.TryMap(request, out _, out var validation);

        Assert.False(success);
        Assert.False(validation.IsValid);
        Assert.Contains("sex", validation.Errors.Keys);
        Assert.Contains("age", validation.Errors.Keys);
        Assert.Contains("activityLevel", validation.Errors.Keys);
        Assert.Contains("goal", validation.Errors.Keys);
    }

    [Fact]
    public void TryMap_ValidRequest_ReturnsMappedInput()
    {
        var mapper = new MacroRequestMapper();

        var request = new CalculateMacrosRequest(
            Sex: "male",
            Age: 29,
            HeightCm: 180,
            WeightKg: 76,
            ActivityLevel: "moderate",
            Goal: "cut"
        );

        var success = mapper.TryMap(request, out var input, out var validation);

        Assert.True(success);
        Assert.True(validation.IsValid);
        Assert.Equal(Sex.Male, input.Sex);
        Assert.Equal(ActivityLevel.Moderate, input.ActivityLevel);
        Assert.Equal(Goal.Cut, input.Goal);
    }
}
