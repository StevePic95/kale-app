namespace Kale.Api.Dtos;

// --- Request DTOs ---

public class GenerateMealPlanRequest
{
    public List<HouseholdMemberDto> Members { get; set; } = new();
}

public class RegenerateMealPlanRequest
{
    public List<HouseholdMemberDto> Members { get; set; } = new();
    public List<VetoDto> Vetoes { get; set; } = new();
}

public class HouseholdMemberDto
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Sex { get; set; } = "male";
    public decimal HeightCm { get; set; }
    public decimal WeightKg { get; set; }
    public string ActivityLevel { get; set; } = "moderate";
    public string[] Allergies { get; set; } = Array.Empty<string>();
    public string[] Likes { get; set; } = Array.Empty<string>();
    public string[] Dislikes { get; set; } = Array.Empty<string>();
}

public class VetoDto
{
    public int DayIndex { get; set; }
    public string MealType { get; set; } = string.Empty;
    public int RecipeId { get; set; }
}

// --- Response DTOs ---

public class MealPlanResponse
{
    public Guid Id { get; set; }
    public List<MealPlanDayDto> Days { get; set; } = new();
    public List<MealSlotDto> Snacks { get; set; } = new();
    public List<ShoppingListItemDto> ShoppingList { get; set; } = new();
    public decimal EstimatedTotalCost { get; set; }
    public List<DailyNutrientSummaryDto> DailyNutrientSummary { get; set; } = new();
}

public class MealPlanDayDto
{
    public string DayOfWeek { get; set; } = string.Empty;
    public MealSlotDto Breakfast { get; set; } = null!;
    public MealSlotDto Dinner { get; set; } = null!;
}

public class MealSlotDto
{
    public int RecipeId { get; set; }
    public string RecipeName { get; set; } = string.Empty;
    public int Servings { get; set; }
    public int PrepTimeMinutes { get; set; }
    public int CookTimeMinutes { get; set; }
    public List<MealIngredientDto> Ingredients { get; set; } = new();
    public string Instructions { get; set; } = string.Empty;
    public NutrientInfoDto NutrientsPerServing { get; set; } = new();
}

public class MealIngredientDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
}

public class NutrientInfoDto
{
    public decimal Calories { get; set; }
    public decimal ProteinG { get; set; }
    public decimal CarbsG { get; set; }
    public decimal FatG { get; set; }
    public decimal FiberG { get; set; }
    public decimal VitaminAMcg { get; set; }
    public decimal VitaminCMg { get; set; }
    public decimal VitaminDMcg { get; set; }
    public decimal VitaminKMcg { get; set; }
    public decimal CalciumMg { get; set; }
    public decimal IronMg { get; set; }
    public decimal PotassiumMg { get; set; }
    public decimal SodiumMg { get; set; }
}

public class ShoppingListItemDto
{
    public string IngredientName { get; set; } = string.Empty;
    public decimal TotalQuantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public decimal EstimatedCost { get; set; }
}

public class DailyNutrientSummaryDto
{
    public string DayOfWeek { get; set; } = string.Empty;
    public decimal TotalCalories { get; set; }
    public decimal TotalProteinG { get; set; }
    public decimal TotalCarbsG { get; set; }
    public decimal TotalFatG { get; set; }
    public decimal TotalFiberG { get; set; }
}
