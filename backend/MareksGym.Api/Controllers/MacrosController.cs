using MareksGym.Api.Application.Macros;
using MareksGym.Api.Contracts.Macros;
using Microsoft.AspNetCore.Mvc;
using MareksGym.Api.Contracts.Macros.History;

namespace MareksGym.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MacrosController : ControllerBase
{
    private readonly MacroCalculator _calculator;
    private readonly MacroRequestMapper _mapper;
    private readonly MacroHistoryService _history;
    private readonly MacroHistoryQueryService _historyQuery;

    public MacrosController(
      MacroCalculator calculator,
      MacroRequestMapper mapper,
      MacroHistoryService history,
      MacroHistoryQueryService historyQuery)
    {
        _calculator = calculator;
        _mapper = mapper;
        _history = history;
        _historyQuery = historyQuery;
    }


    [HttpPost("calculate")]
    public async Task<ActionResult<CalculateMacrosResponse>> Calculate(
        [FromBody] CalculateMacrosRequest request,
        CancellationToken ct)
    {
        if (!_mapper.TryMap(request, out var input, out var validation))
            return BadRequest(new { errors = validation.Errors });

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

    [HttpGet("history")]
    public async Task<ActionResult<GetMacroHistoryResponse>> History(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20,
    CancellationToken ct = default)
    {
        var response = await _historyQuery.GetAsync(page, pageSize, ct);
        return Ok(response);
    }

}