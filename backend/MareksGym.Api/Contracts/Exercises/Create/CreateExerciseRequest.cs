namespace MareksGym.Api.Contracts.Exercises.Create;

public record CreateExerciseRequest(
    string Name,
    string? Description,
    string? MuscleGroup,
    string? Equipment
);