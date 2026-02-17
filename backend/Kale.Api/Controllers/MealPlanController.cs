using Kale.Api.Dtos;
using Kale.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kale.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MealPlanController : ControllerBase
{
    private readonly MealPlannerService _mealPlannerService;

    public MealPlanController(MealPlannerService mealPlannerService)
    {
        _mealPlannerService = mealPlannerService;
    }

    [HttpPost("generate")]
    public async Task<ActionResult<MealPlanResponse>> Generate([FromBody] GenerateMealPlanRequest request)
    {
        if (request.Members == null || request.Members.Count == 0)
        {
            return BadRequest("At least one household member is required.");
        }

        var result = await _mealPlannerService.GenerateMealPlanAsync(request.Members);
        return Ok(result);
    }

    [HttpPost("regenerate")]
    public async Task<ActionResult<MealPlanResponse>> Regenerate([FromBody] RegenerateMealPlanRequest request)
    {
        if (request.Members == null || request.Members.Count == 0)
        {
            return BadRequest("At least one household member is required.");
        }

        var result = await _mealPlannerService.GenerateMealPlanAsync(request.Members, request.Vetoes);
        return Ok(result);
    }
}
