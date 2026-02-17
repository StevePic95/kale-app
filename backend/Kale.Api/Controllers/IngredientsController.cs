using Kale.Api.Data;
using Kale.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kale.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientsController : ControllerBase
{
    private readonly KaleDbContext _context;

    public IngredientsController(KaleDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Ingredient>>> GetAll()
    {
        var ingredients = await _context.Ingredients
            .OrderBy(i => i.Name)
            .ToListAsync();

        return Ok(ingredients);
    }
}
