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

    public MacrosController(MacroCalculator calculator, MacroRequestMapper mapper)
    {
        _calculator = calculator;
        _mapper = mapper;
    }

    [HttpPost("calculate")]
    public ActionResult<CalculateMacrosResponse> Calculate([FromBody] CalculateMacrosRequest request)
    {
        if (!_mapper.TryMap(request, out var input, out var validation))
            return BadRequest(validation);

        var result = _calculator.Calculate(input);

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