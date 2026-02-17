namespace Kale.Api.Models;

public class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal CostPerUnit { get; set; }
    public string Unit { get; set; } = string.Empty;
    public decimal DefaultQuantity { get; set; }

    // Macronutrients per 100g
    public decimal Calories { get; set; }
    public decimal ProteinG { get; set; }
    public decimal CarbsG { get; set; }
    public decimal FatG { get; set; }
    public decimal FiberG { get; set; }

    // Micronutrients per 100g
    public decimal VitaminAMcg { get; set; }
    public decimal VitaminCMg { get; set; }
    public decimal VitaminDMcg { get; set; }
    public decimal VitaminKMcg { get; set; }
    public decimal CalciumMg { get; set; }
    public decimal IronMg { get; set; }
    public decimal PotassiumMg { get; set; }
    public decimal SodiumMg { get; set; }

    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}
