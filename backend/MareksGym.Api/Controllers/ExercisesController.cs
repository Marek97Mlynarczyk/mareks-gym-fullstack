using MareksGym.Api.Application.Exercises;
using Microsoft.AspNetCore.Mvc;

namespace MareksGym.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExercisesController : ControllerBase
{
    private readonly ExerciseQueryService _queryService;

    public ExercisesController(ExerciseQueryService queryService)
    {
        _queryService = queryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetExercises(
        [FromQuery] string? search,
        [FromQuery] string? muscleGroup,
        [FromQuery] string? equipment,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _queryService.GetExercisesAsync(
            search, muscleGroup, equipment, page, pageSize, ct);

        return Ok(result);
    }
}
