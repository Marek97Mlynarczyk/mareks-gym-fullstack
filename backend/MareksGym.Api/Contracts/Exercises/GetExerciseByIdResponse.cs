namespace MareksGym.Api.Contracts.Exercises;

public record GetExerciseByIdResponse(
    int Id,
    string Name,
    string? Description,
    string? MuscleGroup,
    string? Equipment
);