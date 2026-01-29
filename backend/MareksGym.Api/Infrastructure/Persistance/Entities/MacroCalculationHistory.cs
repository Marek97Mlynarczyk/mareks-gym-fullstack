namespace MareksGym.Api.Infrastructure.Persistence.Entities;

// Used to save macro calculation inputs and results so they don’t have to be recalculated every time

public class MacroCalculationHistory
{
    public int Id { get; set; }

    // Original input
    public string Sex { get; set; } = null!;
    public int Age { get; set; }
    public int HeightCm { get; set; }
    public double WeightKg { get; set; }
    public string ActivityLevel { get; set; } = null!;
    public string Goal { get; set; } = null!;

    // Calculated results
    public double Bmr { get; set; }
    public double Tdee { get; set; }
    public double TargetCalories { get; set; }
    public double ProteinGrams { get; set; }
    public double CarbsGrams { get; set; }
    public double FatGrams { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}