using MareksGym.Api.Application.Exercises;
using MareksGym.Api.Application.Exercises.Create;
using MareksGym.Api.Contracts.Exercises.Create;
using Microsoft.AspNetCore.Mvc;
using MareksGym.Api.Application.Exercises.Update;
using MareksGym.Api.Application.Exercises.Delete;
using MareksGym.Api.Contracts.Exercises.Update;

namespace MareksGym.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExercisesController : ControllerBase
{
    private readonly ExerciseQueryService _queryService;
    private readonly ExerciseCreateService _createService;
    private readonly ExerciseSearchStoredProcEfService _searchSpEfService;
    private readonly ExerciseUpdateService _updateService;
    private readonly ExerciseDeleteService _deleteService;

    public ExercisesController(ExerciseQueryService queryService, ExerciseCreateService createService, ExerciseSearchStoredProcEfService searchSpEfService, 
           ExerciseUpdateService updateService, ExerciseDeleteService deleteService)
    {
        _queryService = queryService;
        _createService = createService;
        _searchSpEfService = searchSpEfService;
        _updateService = updateService;
        _deleteService = deleteService;
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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetExerciseById(int id, CancellationToken ct)
    {
        var result = await _queryService.GetExerciseByIdAsync(id, ct);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateExercise([FromBody] CreateExerciseRequest? request, CancellationToken ct)
    {
        var (validation, created) = await _createService.CreateAsync(request, ct);

        if (!validation.IsValid || created is null)
        {
            return BadRequest(new { errors = validation.Errors });
        }

        return CreatedAtAction(nameof(GetExerciseById), new { id = created.Id }, created);
    }

    [HttpGet("search-sp-ef")]
    public async Task<IActionResult> SearchExercisesStoredProcEf(
    [FromQuery] string? search,
    [FromQuery] string? muscleGroup,
    [FromQuery] string? equipment,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20,
    CancellationToken ct = default)
    {
        var result = await _searchSpEfService.SearchAsync(
            search, muscleGroup, equipment, page, pageSize, ct);

        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateExercise(
    int id,
    [FromBody] UpdateExerciseRequest? request,
    CancellationToken ct)
    {
        var (validation, updated) = await _updateService.UpdateAsync(id, request, ct);

        if (updated is null)
        {
            // if not found
            if (validation.Errors.ContainsKey("id"))
                return NotFound();

            return BadRequest(new { errors = validation.Errors });
        }

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteExercise(int id, CancellationToken ct)
    {
        var deleted = await _deleteService.DeleteAsync(id, ct);
        if (!deleted)
            return NotFound();

        return NoContent();
    }

}