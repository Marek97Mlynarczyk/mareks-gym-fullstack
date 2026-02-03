namespace MareksGym.Api.Infrastructure.Persistence.Entities;

public class Exercise
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? MuscleGroup { get; set; }
    public string? Equipment { get; set; }

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}