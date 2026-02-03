namespace MareksGym.Api.Contracts.Exercises;

public record GetExercisesResponse(
    int Id,
    string Name,
    string? MuscleGroup,
    string? Equipment
);
