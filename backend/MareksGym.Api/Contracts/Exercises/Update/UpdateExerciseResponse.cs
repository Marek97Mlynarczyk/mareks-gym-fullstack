namespace MareksGym.Api.Contracts.Exercises.Update;

public record UpdateExerciseResponse(
    int Id,
    string Name,
    string? Description,
    string? MuscleGroup,
    string? Equipment
);