using MareksGym.Api.Application.Macros;
using MareksGym.Api.Contracts.Macros;
using Microsoft.AspNetCore.Mvc;

namespace MareksGym.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MacrosController : ControllerBase
{
    private readonly MacroCalculator _calculator;
    private readonly MacroRequestMapper _mapper;
    private readonly MacroHistoryService _history;
    public MacrosController(
      MacroCalculator calculator,
      MacroRequestMapper mapper,
      MacroHistoryService history)
    {
        _calculator = calculator;
        _mapper = mapper;
        _history = history;
    }

    [HttpPost("calculate")]
    public async Task<ActionResult<CalculateMacrosResponse>> Calculate(
        [FromBody] CalculateMacrosRequest request,
        CancellationToken ct)
    {
        if (!_mapper.TryMap(request, out var input, out var validation))
            return BadRequest(validation);

        var result = _calculator.Calculate(input);

        await _history.SaveAsync(input, result, ct);

        return Ok(new CalculateMacrosResponse(
            Bmr: result.Bmr,
            Tdee: result.Tdee,
            TargetCalories: result.TargetCalories,
            ProteinGrams: result.ProteinGrams,
            CarbsGrams: result.CarbsGrams,
            FatGrams: result.FatGrams
        ));
    }
}