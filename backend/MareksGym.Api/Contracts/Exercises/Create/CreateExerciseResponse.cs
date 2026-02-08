namespace MareksGym.Api.Contracts.Exercises.Create;

public record CreateExerciseResponse(
    int Id,
    string Name,
    string? Description,
    string? MuscleGroup,
    string? Equipment
);