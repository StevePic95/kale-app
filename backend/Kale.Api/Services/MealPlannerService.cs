using Kale.Api.Data;
using Kale.Api.Dtos;
using Kale.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Kale.Api.Services;

public class MealPlannerService
{
    private readonly KaleDbContext _context;

    private static readonly string[] DaysOfWeek =
        { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

    public MealPlannerService(KaleDbContext context)
    {
        _context = context;
    }

    public async Task<MealPlanResponse> GenerateMealPlanAsync(
        List<HouseholdMemberDto> members,
        List<VetoDto>? vetoes = null)
    {
        vetoes ??= new List<VetoDto>();

        // Load all recipes with their ingredients
        var allRecipes = await _context.Recipes
            .Include(r => r.RecipeIngredients)
            .ThenInclude(ri => ri.Ingredient)
            .ToListAsync();

        // Calculate household nutritional targets
        var householdTargets = CalculateHouseholdTargets(members);
        int householdSize = members.Count;

        // Collect all allergens and dislikes across all members
        var allAllergens = members
            .SelectMany(m => m.Allergies)
            .Select(a => a.ToLowerInvariant())
            .Distinct()
            .ToHashSet();

        var allDislikes = members
            .SelectMany(m => m.Dislikes)
            .Select(d => d.ToLowerInvariant())
            .Distinct()
            .ToHashSet();

        // Filter recipes: remove those with allergens or disliked ingredients
        var safeRecipes = allRecipes
            .Where(r => !RecipeContainsAny(r, allAllergens) && !RecipeContainsAny(r, allDislikes))
            .ToList();

        var breakfastRecipes = safeRecipes.Where(r => r.MealType == "breakfast").ToList();
        var dinnerRecipes = safeRecipes.Where(r => r.MealType == "dinner").ToList();
        var snackRecipes = safeRecipes.Where(r => r.MealType == "snack").ToList();

        // Build the weekly plan
        var usedBreakfasts = new HashSet<int>();
        var usedDinners = new HashSet<int>();
        var days = new List<MealPlanDayDto>();

        // Track all ingredients for shopping list
        var shoppingAggregator = new Dictionary<int, (Ingredient ingredient, decimal totalQuantity)>();

        // Track daily nutrients for summary
        var dailyNutrients = new List<DailyNutrientSummaryDto>();

        // Collect all vetoed recipe IDs
        var vetoedRecipeIds = vetoes
            .Where(v => v.RecipeId > 0)
            .Select(v => v.RecipeId)
            .ToHashSet();

        for (int dayIndex = 0; dayIndex < 7; dayIndex++)
        {
            string dayName = DaysOfWeek[dayIndex];

            // Check vetoes for this day
            var breakfastVetoed = vetoes
                .Any(v => v.DayIndex == dayIndex && v.MealType.Equals("breakfast", StringComparison.OrdinalIgnoreCase));
            var dinnerVetoed = vetoes
                .Any(v => v.DayIndex == dayIndex && v.MealType.Equals("dinner", StringComparison.OrdinalIgnoreCase));

            // Pick breakfast (exclude vetoed recipes if this slot was vetoed)
            var breakfast = PickRecipe(breakfastRecipes, usedBreakfasts, breakfastVetoed ? vetoedRecipeIds : null);
            if (breakfast != null) usedBreakfasts.Add(breakfast.Id);

            // Pick dinner (exclude vetoed recipes if this slot was vetoed)
            var dinner = PickRecipe(dinnerRecipes, usedDinners, dinnerVetoed ? vetoedRecipeIds : null);
            if (dinner != null) usedDinners.Add(dinner.Id);

            // Scale servings
            int breakfastServings = householdSize;
            // Dinner makes 2x for leftovers (lunch the next day)
            int dinnerServings = householdSize * 2;

            var breakfastSlot = BuildMealSlot(breakfast, breakfastServings, householdTargets, shoppingAggregator);
            var dinnerSlot = BuildMealSlot(dinner, dinnerServings, householdTargets, shoppingAggregator);

            // Calculate daily nutrients (per person)
            var dayNutrients = new DailyNutrientSummaryDto
            {
                DayOfWeek = dayName,
                TotalCalories = (breakfastSlot?.NutrientsPerServing.Calories ?? 0) +
                                (dinnerSlot?.NutrientsPerServing.Calories ?? 0) * 2, // dinner + lunch leftovers
                TotalProteinG = (breakfastSlot?.NutrientsPerServing.ProteinG ?? 0) +
                                (dinnerSlot?.NutrientsPerServing.ProteinG ?? 0) * 2,
                TotalCarbsG = (breakfastSlot?.NutrientsPerServing.CarbsG ?? 0) +
                              (dinnerSlot?.NutrientsPerServing.CarbsG ?? 0) * 2,
                TotalFatG = (breakfastSlot?.NutrientsPerServing.FatG ?? 0) +
                            (dinnerSlot?.NutrientsPerServing.FatG ?? 0) * 2,
                TotalFiberG = (breakfastSlot?.NutrientsPerServing.FiberG ?? 0) +
                              (dinnerSlot?.NutrientsPerServing.FiberG ?? 0) * 2,
            };
            dailyNutrients.Add(dayNutrients);

            days.Add(new MealPlanDayDto
            {
                DayOfWeek = dayName,
                Breakfast = breakfastSlot ?? BuildEmptySlot(),
                Dinner = dinnerSlot ?? BuildEmptySlot(),
            });
        }

        // Pick 2-3 snacks
        var snacks = new List<MealSlotDto>();
        var usedSnacks = new HashSet<int>();
        int snackCount = Math.Min(snackRecipes.Count, 3);
        for (int i = 0; i < snackCount; i++)
        {
            var snack = PickRecipe(snackRecipes, usedSnacks, null);
            if (snack != null)
            {
                usedSnacks.Add(snack.Id);
                int snackServings = householdSize * 2; // Enough for multiple snacking occasions
                var slot = BuildMealSlot(snack, snackServings, householdTargets, shoppingAggregator);
                if (slot != null) snacks.Add(slot);
            }
        }

        // Build shopping list
        var shoppingList = shoppingAggregator.Values
            .Select(entry => new ShoppingListItemDto
            {
                IngredientName = entry.ingredient.Name,
                TotalQuantity = Math.Round(entry.totalQuantity, 1),
                Unit = entry.ingredient.Unit,
                EstimatedCost = CalculateIngredientCost(entry.ingredient, entry.totalQuantity),
            })
            .OrderBy(s => s.IngredientName)
            .ToList();

        decimal totalCost = shoppingList.Sum(s => s.EstimatedCost);

        return new MealPlanResponse
        {
            Id = Guid.NewGuid(),
            Days = days,
            Snacks = snacks,
            ShoppingList = shoppingList,
            EstimatedTotalCost = Math.Round(totalCost, 2),
            DailyNutrientSummary = dailyNutrients,
        };
    }

    /// <summary>
    /// Pick a recipe that hasn't been used yet (or least recently used if we run out).
    /// If there's a veto, the additionalExclusions set is used to exclude previously vetoed recipes.
    /// </summary>
    private static Recipe? PickRecipe(
        List<Recipe> candidates,
        HashSet<int> usedIds,
        HashSet<int>? additionalExclusions)
    {
        if (candidates.Count == 0) return null;

        // First try: pick an unused recipe
        var available = candidates.Where(r => !usedIds.Contains(r.Id)).ToList();

        if (additionalExclusions != null)
        {
            // For vetoed slots, also skip any additional exclusions
            available = available.Where(r => !additionalExclusions.Contains(r.Id)).ToList();
        }

        if (available.Count > 0)
        {
            // Simple selection: pick from available, rotating through them
            return available[usedIds.Count % available.Count];
        }

        // Fallback: reuse from full candidate pool (wrap around)
        var fallback = candidates.Where(r => additionalExclusions == null || !additionalExclusions.Contains(r.Id)).ToList();
        if (fallback.Count == 0) fallback = candidates;
        return fallback[usedIds.Count % fallback.Count];
    }

    /// <summary>
    /// Build a MealSlotDto from a recipe, scaling ingredients to the target servings.
    /// Also adjusts flexible ingredients to better hit nutrient targets.
    /// </summary>
    private MealSlotDto? BuildMealSlot(
        Recipe? recipe,
        int targetServings,
        HouseholdNutrientTargets targets,
        Dictionary<int, (Ingredient ingredient, decimal totalQuantity)> shoppingAggregator)
    {
        if (recipe == null) return null;

        decimal scaleFactor = (decimal)targetServings / recipe.BaseServings;

        var ingredients = new List<MealIngredientDto>();
        var totalNutrients = new NutrientInfoDto();

        foreach (var ri in recipe.RecipeIngredients)
        {
            decimal quantity = ri.Quantity * scaleFactor;

            // For flexible ingredients, adjust toward the max to help hit nutrient targets
            if (ri.FlexibilityType == "flexible" && ri.MinQuantity.HasValue && ri.MaxQuantity.HasValue)
            {
                decimal minScaled = ri.MinQuantity.Value * scaleFactor;
                decimal maxScaled = ri.MaxQuantity.Value * scaleFactor;
                // Use 70% of the range as a reasonable default bump toward nutrient goals
                quantity = minScaled + (maxScaled - minScaled) * 0.7m;
            }

            // For additions, include them at their specified quantity
            if (ri.FlexibilityType == "addition")
            {
                quantity = ri.Quantity * scaleFactor;
            }

            quantity = Math.Round(quantity, 1);

            ingredients.Add(new MealIngredientDto
            {
                Name = ri.Ingredient.Name,
                Quantity = quantity,
                Unit = ri.Unit,
            });

            // Accumulate nutrients for this meal
            // Nutrients in the DB are per 100g; convert based on quantity
            decimal nutrientFactor = GetNutrientFactor(ri.Ingredient, quantity, ri.Unit);
            totalNutrients.Calories += ri.Ingredient.Calories * nutrientFactor;
            totalNutrients.ProteinG += ri.Ingredient.ProteinG * nutrientFactor;
            totalNutrients.CarbsG += ri.Ingredient.CarbsG * nutrientFactor;
            totalNutrients.FatG += ri.Ingredient.FatG * nutrientFactor;
            totalNutrients.FiberG += ri.Ingredient.FiberG * nutrientFactor;
            totalNutrients.VitaminAMcg += ri.Ingredient.VitaminAMcg * nutrientFactor;
            totalNutrients.VitaminCMg += ri.Ingredient.VitaminCMg * nutrientFactor;
            totalNutrients.VitaminDMcg += ri.Ingredient.VitaminDMcg * nutrientFactor;
            totalNutrients.VitaminKMcg += ri.Ingredient.VitaminKMcg * nutrientFactor;
            totalNutrients.CalciumMg += ri.Ingredient.CalciumMg * nutrientFactor;
            totalNutrients.IronMg += ri.Ingredient.IronMg * nutrientFactor;
            totalNutrients.PotassiumMg += ri.Ingredient.PotassiumMg * nutrientFactor;
            totalNutrients.SodiumMg += ri.Ingredient.SodiumMg * nutrientFactor;

            // Add to shopping list aggregator
            AddToShoppingAggregator(shoppingAggregator, ri.Ingredient, quantity);
        }

        // Convert total nutrients to per-serving
        if (targetServings > 0)
        {
            totalNutrients.Calories = Math.Round(totalNutrients.Calories / targetServings, 1);
            totalNutrients.ProteinG = Math.Round(totalNutrients.ProteinG / targetServings, 1);
            totalNutrients.CarbsG = Math.Round(totalNutrients.CarbsG / targetServings, 1);
            totalNutrients.FatG = Math.Round(totalNutrients.FatG / targetServings, 1);
            totalNutrients.FiberG = Math.Round(totalNutrients.FiberG / targetServings, 1);
            totalNutrients.VitaminAMcg = Math.Round(totalNutrients.VitaminAMcg / targetServings, 1);
            totalNutrients.VitaminCMg = Math.Round(totalNutrients.VitaminCMg / targetServings, 1);
            totalNutrients.VitaminDMcg = Math.Round(totalNutrients.VitaminDMcg / targetServings, 1);
            totalNutrients.VitaminKMcg = Math.Round(totalNutrients.VitaminKMcg / targetServings, 1);
            totalNutrients.CalciumMg = Math.Round(totalNutrients.CalciumMg / targetServings, 1);
            totalNutrients.IronMg = Math.Round(totalNutrients.IronMg / targetServings, 1);
            totalNutrients.PotassiumMg = Math.Round(totalNutrients.PotassiumMg / targetServings, 1);
            totalNutrients.SodiumMg = Math.Round(totalNutrients.SodiumMg / targetServings, 1);
        }

        return new MealSlotDto
        {
            RecipeId = recipe.Id,
            RecipeName = recipe.Name,
            Servings = targetServings,
            PrepTimeMinutes = recipe.PrepTimeMinutes,
            CookTimeMinutes = recipe.CookTimeMinutes,
            Ingredients = ingredients,
            Instructions = recipe.Instructions,
            NutrientsPerServing = totalNutrients,
        };
    }

    /// <summary>
    /// Returns the nutrient multiplier. Nutrients are stored per 100g.
    /// For gram-based units, factor = quantity / 100.
    /// For ml-based units, same approach (approximation).
    /// For "each" units (eggs, lemons), use a rough weight estimate.
    /// </summary>
    private static decimal GetNutrientFactor(Ingredient ingredient, decimal quantity, string unit)
    {
        string unitLower = unit.ToLowerInvariant();

        if (unitLower == "g" || unitLower == "ml")
        {
            return quantity / 100m;
        }

        if (unitLower == "each")
        {
            // Rough weight estimates for "each" items
            decimal estimatedWeightG = ingredient.Name.ToLowerInvariant() switch
            {
                "eggs" => 50m,       // ~50g per large egg
                "lemons" => 60m,     // ~60g juice + zest per lemon
                _ => 100m,           // fallback
            };
            return quantity * estimatedWeightG / 100m;
        }

        // Fallback
        return quantity / 100m;
    }

    private static void AddToShoppingAggregator(
        Dictionary<int, (Ingredient ingredient, decimal totalQuantity)> aggregator,
        Ingredient ingredient,
        decimal quantity)
    {
        if (aggregator.TryGetValue(ingredient.Id, out var existing))
        {
            aggregator[ingredient.Id] = (existing.ingredient, existing.totalQuantity + quantity);
        }
        else
        {
            aggregator[ingredient.Id] = (ingredient, quantity);
        }
    }

    /// <summary>
    /// Calculate cost based on quantity needed vs. default purchase quantity.
    /// </summary>
    private static decimal CalculateIngredientCost(Ingredient ingredient, decimal totalQuantity)
    {
        if (ingredient.DefaultQuantity <= 0) return 0;

        // How many "units" of this ingredient do we need to buy?
        decimal unitsNeeded = totalQuantity / ingredient.DefaultQuantity;

        // Round up to the nearest whole unit for purchasing
        decimal wholeUnits = Math.Ceiling(unitsNeeded);

        return Math.Round(wholeUnits * ingredient.CostPerUnit, 2);
    }

    /// <summary>
    /// Check if any ingredient in the recipe matches an allergen/dislike keyword.
    /// </summary>
    private static bool RecipeContainsAny(Recipe recipe, HashSet<string> keywords)
    {
        if (keywords.Count == 0) return false;

        foreach (var ri in recipe.RecipeIngredients)
        {
            string ingredientNameLower = ri.Ingredient.Name.ToLowerInvariant();
            foreach (var keyword in keywords)
            {
                if (ingredientNameLower.Contains(keyword))
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Calculates combined household calorie/macro targets using the Mifflin-St Jeor equation.
    /// </summary>
    private static HouseholdNutrientTargets CalculateHouseholdTargets(List<HouseholdMemberDto> members)
    {
        var targets = new HouseholdNutrientTargets();

        foreach (var member in members)
        {
            // Mifflin-St Jeor BMR
            decimal bmr;
            if (member.Sex.Equals("male", StringComparison.OrdinalIgnoreCase))
            {
                bmr = 10m * member.WeightKg + 6.25m * member.HeightCm - 5m * member.Age + 5m;
            }
            else
            {
                bmr = 10m * member.WeightKg + 6.25m * member.HeightCm - 5m * member.Age - 161m;
            }

            decimal activityMultiplier = member.ActivityLevel.ToLowerInvariant() switch
            {
                "sedentary" => 1.2m,
                "light" => 1.375m,
                "moderate" => 1.55m,
                "active" => 1.725m,
                _ => 1.55m,
            };

            decimal dailyCalories = bmr * activityMultiplier;

            targets.TotalDailyCalories += dailyCalories;

            // Standard macro split: 30% protein, 40% carbs, 30% fat
            targets.TotalDailyProteinG += dailyCalories * 0.30m / 4m;  // 4 cal per gram protein
            targets.TotalDailyCarbsG += dailyCalories * 0.40m / 4m;    // 4 cal per gram carbs
            targets.TotalDailyFatG += dailyCalories * 0.30m / 9m;      // 9 cal per gram fat
        }

        return targets;
    }

    private static MealSlotDto BuildEmptySlot()
    {
        return new MealSlotDto
        {
            RecipeId = 0,
            RecipeName = "No recipe available",
            Servings = 0,
            PrepTimeMinutes = 0,
            CookTimeMinutes = 0,
            Ingredients = new List<MealIngredientDto>(),
            Instructions = string.Empty,
            NutrientsPerServing = new NutrientInfoDto(),
        };
    }
}

public class HouseholdNutrientTargets
{
    public decimal TotalDailyCalories { get; set; }
    public decimal TotalDailyProteinG { get; set; }
    public decimal TotalDailyCarbsG { get; set; }
    public decimal TotalDailyFatG { get; set; }
}
