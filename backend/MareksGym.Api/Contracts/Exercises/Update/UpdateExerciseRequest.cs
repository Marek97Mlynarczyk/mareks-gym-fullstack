namespace MareksGym.Api.Contracts.Exercises.Update;

public record UpdateExerciseRequest(
    string Name,
    string? Description,
    string? MuscleGroup,
    string? Equipment
);