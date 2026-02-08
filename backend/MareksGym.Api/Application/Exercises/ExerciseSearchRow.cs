namespace MareksGym.Api.Application.Exercises;

// This class is used only to map stored procedure results
public class ExerciseSearchRow
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string? MuscleGroup { get; set; }
    public string? Equipment { get; set; }
}