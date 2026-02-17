using Kale.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kale.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private readonly KaleDbContext _context;

    public RecipesController(KaleDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var recipes = await _context.Recipes
            .Include(r => r.RecipeIngredients)
            .ThenInclude(ri => ri.Ingredient)
            .OrderBy(r => r.Name)
            .Select(r => new
            {
                r.Id,
                r.Name,
                r.MealType,
                r.PrepTimeMinutes,
                r.CookTimeMinutes,
                r.BaseServings,
                r.Instructions,
                r.DishTags,
                Ingredients = r.RecipeIngredients.Select(ri => new
                {
                    ri.Id,
                    ri.Ingredient.Name,
                    ri.Quantity,
                    ri.Unit,
                    ri.FlexibilityType,
                    ri.MinQuantity,
                    ri.MaxQuantity,
                }).ToList(),
            })
            .ToListAsync();

        return Ok(recipes);
    }
}
