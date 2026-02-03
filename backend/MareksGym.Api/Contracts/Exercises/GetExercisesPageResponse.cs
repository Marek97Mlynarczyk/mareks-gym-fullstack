namespace MareksGym.Api.Contracts.Exercises;

public record GetExercisesPageResponse(
    IReadOnlyList<GetExercisesResponse> Items,
    int Page,
    int PageSize,
    int TotalCount
);
