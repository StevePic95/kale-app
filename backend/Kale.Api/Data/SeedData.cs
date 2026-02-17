using Microsoft.EntityFrameworkCore;
using Kale.Api.Models;

namespace Kale.Api.Data;

public static class SeedData
{
    public static void Initialize(KaleDbContext context)
    {
        if (context.Ingredients.Any())
            return;

        var ingredients = GetIngredients();
        context.Ingredients.AddRange(ingredients);
        context.SaveChanges();

        var recipes = GetRecipes();
        context.Recipes.AddRange(recipes);
        context.SaveChanges();

        var recipeIngredients = GetRecipeIngredients();
        context.RecipeIngredients.AddRange(recipeIngredients);
        context.SaveChanges();

        // Reset Postgres identity sequences so future inserts don't collide with seed IDs
        var maxIngredientId = context.Ingredients.Max(i => i.Id);
        var maxRecipeId = context.Recipes.Max(r => r.Id);
        var maxRecipeIngredientId = context.RecipeIngredients.Max(ri => ri.Id);

        context.Database.ExecuteSqlRaw(
            "ALTER TABLE \"Ingredients\" ALTER COLUMN \"Id\" RESTART WITH " + (maxIngredientId + 1));
        context.Database.ExecuteSqlRaw(
            "ALTER TABLE \"Recipes\" ALTER COLUMN \"Id\" RESTART WITH " + (maxRecipeId + 1));
        context.Database.ExecuteSqlRaw(
            "ALTER TABLE \"RecipeIngredients\" ALTER COLUMN \"Id\" RESTART WITH " + (maxRecipeIngredientId + 1));
    }

    private static List<Ingredient> GetIngredients()
    {
        return new List<Ingredient>
        {
            // Proteins
            new Ingredient
            {
                Id = 1, Name = "Chicken Breast", Category = "protein",
                CostPerUnit = 3.99m, Unit = "g", DefaultQuantity = 500m,
                Calories = 165m, ProteinG = 31m, CarbsG = 0m, FatG = 3.6m, FiberG = 0m,
                VitaminAMcg = 6m, VitaminCMg = 0m, VitaminDMcg = 0.1m, VitaminKMcg = 0m,
                CalciumMg = 15m, IronMg = 1.0m, PotassiumMg = 256m, SodiumMg = 74m
            },
            new Ingredient
            {
                Id = 2, Name = "Salmon", Category = "protein",
                CostPerUnit = 8.99m, Unit = "g", DefaultQuantity = 400m,
                Calories = 208m, ProteinG = 20m, CarbsG = 0m, FatG = 13m, FiberG = 0m,
                VitaminAMcg = 12m, VitaminCMg = 0m, VitaminDMcg = 11m, VitaminKMcg = 0.5m,
                CalciumMg = 12m, IronMg = 0.8m, PotassiumMg = 363m, SodiumMg = 59m
            },
            new Ingredient
            {
                Id = 3, Name = "Lentils", Category = "protein",
                CostPerUnit = 1.99m, Unit = "g", DefaultQuantity = 500m,
                Calories = 116m, ProteinG = 9m, CarbsG = 20m, FatG = 0.4m, FiberG = 7.9m,
                VitaminAMcg = 8m, VitaminCMg = 1.5m, VitaminDMcg = 0m, VitaminKMcg = 1.7m,
                CalciumMg = 19m, IronMg = 3.3m, PotassiumMg = 369m, SodiumMg = 2m
            },
            new Ingredient
            {
                Id = 4, Name = "Chickpeas", Category = "protein",
                CostPerUnit = 1.49m, Unit = "g", DefaultQuantity = 400m,
                Calories = 164m, ProteinG = 8.9m, CarbsG = 27m, FatG = 2.6m, FiberG = 7.6m,
                VitaminAMcg = 1m, VitaminCMg = 1.3m, VitaminDMcg = 0m, VitaminKMcg = 4m,
                CalciumMg = 49m, IronMg = 2.9m, PotassiumMg = 291m, SodiumMg = 7m
            },
            new Ingredient
            {
                Id = 5, Name = "Eggs", Category = "protein",
                CostPerUnit = 3.49m, Unit = "each", DefaultQuantity = 12m,
                Calories = 155m, ProteinG = 13m, CarbsG = 1.1m, FatG = 11m, FiberG = 0m,
                VitaminAMcg = 160m, VitaminCMg = 0m, VitaminDMcg = 2m, VitaminKMcg = 0.3m,
                CalciumMg = 56m, IronMg = 1.8m, PotassiumMg = 138m, SodiumMg = 124m
            },

            // Vegetables
            new Ingredient
            {
                Id = 6, Name = "Spinach", Category = "vegetable",
                CostPerUnit = 2.99m, Unit = "g", DefaultQuantity = 300m,
                Calories = 23m, ProteinG = 2.9m, CarbsG = 3.6m, FatG = 0.4m, FiberG = 2.2m,
                VitaminAMcg = 469m, VitaminCMg = 28m, VitaminDMcg = 0m, VitaminKMcg = 483m,
                CalciumMg = 99m, IronMg = 2.7m, PotassiumMg = 558m, SodiumMg = 79m
            },
            new Ingredient
            {
                Id = 7, Name = "Tomatoes", Category = "vegetable",
                CostPerUnit = 1.99m, Unit = "g", DefaultQuantity = 500m,
                Calories = 18m, ProteinG = 0.9m, CarbsG = 3.9m, FatG = 0.2m, FiberG = 1.2m,
                VitaminAMcg = 42m, VitaminCMg = 14m, VitaminDMcg = 0m, VitaminKMcg = 7.9m,
                CalciumMg = 10m, IronMg = 0.3m, PotassiumMg = 237m, SodiumMg = 5m
            },
            new Ingredient
            {
                Id = 8, Name = "Bell Peppers", Category = "vegetable",
                CostPerUnit = 1.50m, Unit = "g", DefaultQuantity = 200m,
                Calories = 31m, ProteinG = 1.0m, CarbsG = 6m, FatG = 0.3m, FiberG = 2.1m,
                VitaminAMcg = 157m, VitaminCMg = 128m, VitaminDMcg = 0m, VitaminKMcg = 4.9m,
                CalciumMg = 7m, IronMg = 0.4m, PotassiumMg = 211m, SodiumMg = 4m
            },
            new Ingredient
            {
                Id = 9, Name = "Zucchini", Category = "vegetable",
                CostPerUnit = 1.29m, Unit = "g", DefaultQuantity = 300m,
                Calories = 17m, ProteinG = 1.2m, CarbsG = 3.1m, FatG = 0.3m, FiberG = 1.0m,
                VitaminAMcg = 10m, VitaminCMg = 18m, VitaminDMcg = 0m, VitaminKMcg = 4.3m,
                CalciumMg = 16m, IronMg = 0.4m, PotassiumMg = 261m, SodiumMg = 8m
            },
            new Ingredient
            {
                Id = 10, Name = "Kale", Category = "vegetable",
                CostPerUnit = 2.49m, Unit = "g", DefaultQuantity = 300m,
                Calories = 49m, ProteinG = 4.3m, CarbsG = 8.8m, FatG = 0.9m, FiberG = 3.6m,
                VitaminAMcg = 500m, VitaminCMg = 120m, VitaminDMcg = 0m, VitaminKMcg = 817m,
                CalciumMg = 150m, IronMg = 1.5m, PotassiumMg = 491m, SodiumMg = 38m
            },
            new Ingredient
            {
                Id = 11, Name = "Onions", Category = "vegetable",
                CostPerUnit = 1.29m, Unit = "g", DefaultQuantity = 500m,
                Calories = 40m, ProteinG = 1.1m, CarbsG = 9.3m, FatG = 0.1m, FiberG = 1.7m,
                VitaminAMcg = 0m, VitaminCMg = 7.4m, VitaminDMcg = 0m, VitaminKMcg = 0.4m,
                CalciumMg = 23m, IronMg = 0.2m, PotassiumMg = 146m, SodiumMg = 4m
            },
            new Ingredient
            {
                Id = 12, Name = "Garlic", Category = "vegetable",
                CostPerUnit = 0.50m, Unit = "g", DefaultQuantity = 30m,
                Calories = 149m, ProteinG = 6.4m, CarbsG = 33m, FatG = 0.5m, FiberG = 2.1m,
                VitaminAMcg = 0m, VitaminCMg = 31m, VitaminDMcg = 0m, VitaminKMcg = 1.7m,
                CalciumMg = 181m, IronMg = 1.7m, PotassiumMg = 401m, SodiumMg = 17m
            },

            // Grains
            new Ingredient
            {
                Id = 13, Name = "Brown Rice", Category = "grain",
                CostPerUnit = 2.49m, Unit = "g", DefaultQuantity = 1000m,
                Calories = 112m, ProteinG = 2.6m, CarbsG = 24m, FatG = 0.9m, FiberG = 1.8m,
                VitaminAMcg = 0m, VitaminCMg = 0m, VitaminDMcg = 0m, VitaminKMcg = 0.2m,
                CalciumMg = 10m, IronMg = 0.4m, PotassiumMg = 43m, SodiumMg = 1m
            },
            new Ingredient
            {
                Id = 14, Name = "Whole Wheat Pasta", Category = "grain",
                CostPerUnit = 1.99m, Unit = "g", DefaultQuantity = 500m,
                Calories = 124m, ProteinG = 5.3m, CarbsG = 27m, FatG = 0.5m, FiberG = 3.9m,
                VitaminAMcg = 0m, VitaminCMg = 0m, VitaminDMcg = 0m, VitaminKMcg = 0.1m,
                CalciumMg = 15m, IronMg = 1.4m, PotassiumMg = 44m, SodiumMg = 1m
            },

            // Other
            new Ingredient
            {
                Id = 15, Name = "Olive Oil", Category = "oil",
                CostPerUnit = 6.99m, Unit = "ml", DefaultQuantity = 500m,
                Calories = 884m, ProteinG = 0m, CarbsG = 0m, FatG = 100m, FiberG = 0m,
                VitaminAMcg = 0m, VitaminCMg = 0m, VitaminDMcg = 0m, VitaminKMcg = 60.2m,
                CalciumMg = 1m, IronMg = 0.6m, PotassiumMg = 1m, SodiumMg = 2m
            },
            new Ingredient
            {
                Id = 16, Name = "Feta Cheese", Category = "dairy",
                CostPerUnit = 4.99m, Unit = "g", DefaultQuantity = 200m,
                Calories = 264m, ProteinG = 14m, CarbsG = 4.1m, FatG = 21m, FiberG = 0m,
                VitaminAMcg = 125m, VitaminCMg = 0m, VitaminDMcg = 0.4m, VitaminKMcg = 1.8m,
                CalciumMg = 493m, IronMg = 0.7m, PotassiumMg = 62m, SodiumMg = 917m
            },
            new Ingredient
            {
                Id = 17, Name = "Lemons", Category = "fruit",
                CostPerUnit = 0.50m, Unit = "each", DefaultQuantity = 5m,
                Calories = 29m, ProteinG = 1.1m, CarbsG = 9.3m, FatG = 0.3m, FiberG = 2.8m,
                VitaminAMcg = 1m, VitaminCMg = 53m, VitaminDMcg = 0m, VitaminKMcg = 0m,
                CalciumMg = 26m, IronMg = 0.6m, PotassiumMg = 138m, SodiumMg = 2m
            },
            new Ingredient
            {
                Id = 18, Name = "Greek Yogurt", Category = "dairy",
                CostPerUnit = 4.49m, Unit = "g", DefaultQuantity = 500m,
                Calories = 59m, ProteinG = 10m, CarbsG = 3.6m, FatG = 0.7m, FiberG = 0m,
                VitaminAMcg = 7m, VitaminCMg = 0m, VitaminDMcg = 0m, VitaminKMcg = 0m,
                CalciumMg = 110m, IronMg = 0.1m, PotassiumMg = 141m, SodiumMg = 36m
            },
            new Ingredient
            {
                Id = 19, Name = "Rolled Oats", Category = "grain",
                CostPerUnit = 3.49m, Unit = "g", DefaultQuantity = 500m,
                Calories = 389m, ProteinG = 17m, CarbsG = 66m, FatG = 6.9m, FiberG = 10.6m,
                VitaminAMcg = 0m, VitaminCMg = 0m, VitaminDMcg = 0m, VitaminKMcg = 0m,
                CalciumMg = 54m, IronMg = 4.7m, PotassiumMg = 429m, SodiumMg = 2m
            },
        };
    }

    private static List<Recipe> GetRecipes()
    {
        return new List<Recipe>
        {
            // Breakfasts
            new Recipe
            {
                Id = 1,
                Name = "Greek Yogurt Bowl with Honey and Walnuts",
                MealType = "breakfast",
                PrepTimeMinutes = 5,
                CookTimeMinutes = 0,
                BaseServings = 1,
                DishTags = "bowl,cold",
                Instructions = "1. Scoop Greek yogurt into a bowl.\n2. Drizzle with olive oil or honey.\n3. Top with fresh fruit or a squeeze of lemon.\n4. Add a handful of rolled oats for crunch if desired."
            },
            new Recipe
            {
                Id = 2,
                Name = "Mediterranean Veggie Omelet",
                MealType = "breakfast",
                PrepTimeMinutes = 5,
                CookTimeMinutes = 10,
                BaseServings = 1,
                DishTags = "eggs,skillet",
                Instructions = "1. Whisk eggs in a bowl with a pinch of salt.\n2. Heat olive oil in a non-stick skillet over medium heat.\n3. Saut\u00e9 diced bell peppers, tomatoes, and spinach for 2-3 minutes.\n4. Pour eggs over the vegetables.\n5. Cook until edges set, then fold the omelet.\n6. Top with crumbled feta cheese.\n7. Serve immediately."
            },
            new Recipe
            {
                Id = 3,
                Name = "Overnight Oats with Lemon and Spinach",
                MealType = "breakfast",
                PrepTimeMinutes = 10,
                CookTimeMinutes = 0,
                BaseServings = 1,
                DishTags = "bowl,cold,make-ahead",
                Instructions = "1. Combine rolled oats with Greek yogurt in a jar.\n2. Add a squeeze of lemon juice and a drizzle of olive oil.\n3. Stir in a handful of finely chopped spinach.\n4. Cover and refrigerate overnight.\n5. In the morning, stir and enjoy cold."
            },

            // Dinners
            new Recipe
            {
                Id = 4,
                Name = "Lemon Herb Chicken with Brown Rice",
                MealType = "dinner",
                PrepTimeMinutes = 15,
                CookTimeMinutes = 30,
                BaseServings = 2,
                DishTags = "roasted,plated",
                Instructions = "1. Preheat oven to 400\u00b0F (200\u00b0C).\n2. Season chicken breasts with olive oil, lemon juice, garlic, salt, and pepper.\n3. Place chicken on a baking sheet and roast for 25-30 minutes.\n4. Meanwhile, cook brown rice according to package directions.\n5. Saut\u00e9 zucchini and bell peppers in olive oil until tender.\n6. Serve chicken over rice with saut\u00e9ed vegetables on the side."
            },
            new Recipe
            {
                Id = 5,
                Name = "Mediterranean Lentil Soup",
                MealType = "dinner",
                PrepTimeMinutes = 10,
                CookTimeMinutes = 35,
                BaseServings = 4,
                DishTags = "soup,stew",
                Instructions = "1. Heat olive oil in a large pot over medium heat.\n2. Saut\u00e9 diced onions and garlic until softened, about 3 minutes.\n3. Add diced tomatoes and cook for 2 minutes.\n4. Add rinsed lentils and 4 cups of water or vegetable broth.\n5. Bring to a boil, then reduce to a simmer.\n6. Cook for 25-30 minutes until lentils are tender.\n7. Stir in spinach during the last 5 minutes.\n8. Season with lemon juice, salt, and pepper.\n9. Serve hot with a drizzle of olive oil."
            },
            new Recipe
            {
                Id = 6,
                Name = "Grilled Salmon with Roasted Vegetables",
                MealType = "dinner",
                PrepTimeMinutes = 15,
                CookTimeMinutes = 25,
                BaseServings = 2,
                DishTags = "grilled,plated",
                Instructions = "1. Preheat oven to 425\u00b0F (220\u00b0C).\n2. Toss zucchini, bell peppers, and tomatoes with olive oil, salt, and pepper.\n3. Spread vegetables on a baking sheet and roast for 20 minutes.\n4. Season salmon fillets with olive oil, lemon juice, garlic, salt, and pepper.\n5. Grill or pan-sear salmon for 4-5 minutes per side.\n6. Serve salmon over roasted vegetables.\n7. Garnish with fresh lemon wedges."
            },
            new Recipe
            {
                Id = 7,
                Name = "Chickpea and Vegetable Stir-Fry",
                MealType = "dinner",
                PrepTimeMinutes = 10,
                CookTimeMinutes = 15,
                BaseServings = 2,
                DishTags = "stir-fry,skillet",
                Instructions = "1. Heat olive oil in a large skillet or wok over medium-high heat.\n2. Saut\u00e9 diced onions and garlic for 2 minutes.\n3. Add sliced bell peppers and zucchini, cook for 5 minutes.\n4. Add drained chickpeas and diced tomatoes.\n5. Season with cumin, paprika, salt, and pepper.\n6. Cook for 5 more minutes, stirring frequently.\n7. Serve over brown rice or whole wheat pasta.\n8. Top with crumbled feta cheese if desired."
            },

            // Snack
            new Recipe
            {
                Id = 8,
                Name = "Hummus with Fresh Vegetables",
                MealType = "snack",
                PrepTimeMinutes = 15,
                CookTimeMinutes = 0,
                BaseServings = 4,
                DishTags = "dip,cold",
                Instructions = "1. Drain and rinse chickpeas.\n2. In a food processor, combine chickpeas, olive oil, lemon juice, garlic, and a pinch of salt.\n3. Blend until smooth, adding water as needed for desired consistency.\n4. Transfer to a bowl and drizzle with olive oil.\n5. Serve with sliced bell peppers, zucchini sticks, and tomato wedges."
            },
        };
    }

    private static List<RecipeIngredient> GetRecipeIngredients()
    {
        return new List<RecipeIngredient>
        {
            // Recipe 1: Greek Yogurt Bowl
            new RecipeIngredient { Id = 1, RecipeId = 1, IngredientId = 18, Quantity = 200, Unit = "g", FlexibilityType = "base" },
            new RecipeIngredient { Id = 2, RecipeId = 1, IngredientId = 15, Quantity = 10, Unit = "ml", FlexibilityType = "base" },
            new RecipeIngredient { Id = 3, RecipeId = 1, IngredientId = 19, Quantity = 30, Unit = "g", FlexibilityType = "flexible", MinQuantity = 20, MaxQuantity = 50 },
            new RecipeIngredient { Id = 4, RecipeId = 1, IngredientId = 17, Quantity = 0.5m, Unit = "each", FlexibilityType = "base" },

            // Recipe 2: Mediterranean Veggie Omelet
            new RecipeIngredient { Id = 5, RecipeId = 2, IngredientId = 5, Quantity = 3, Unit = "each", FlexibilityType = "base" },
            new RecipeIngredient { Id = 6, RecipeId = 2, IngredientId = 15, Quantity = 15, Unit = "ml", FlexibilityType = "base" },
            new RecipeIngredient { Id = 7, RecipeId = 2, IngredientId = 8, Quantity = 50, Unit = "g", FlexibilityType = "flexible", MinQuantity = 30, MaxQuantity = 80 },
            new RecipeIngredient { Id = 8, RecipeId = 2, IngredientId = 7, Quantity = 50, Unit = "g", FlexibilityType = "flexible", MinQuantity = 30, MaxQuantity = 80 },
            new RecipeIngredient { Id = 9, RecipeId = 2, IngredientId = 6, Quantity = 30, Unit = "g", FlexibilityType = "flexible", MinQuantity = 20, MaxQuantity = 60 },
            new RecipeIngredient { Id = 10, RecipeId = 2, IngredientId = 16, Quantity = 20, Unit = "g", FlexibilityType = "flexible", MinQuantity = 10, MaxQuantity = 40 },

            // Recipe 3: Overnight Oats
            new RecipeIngredient { Id = 11, RecipeId = 3, IngredientId = 19, Quantity = 60, Unit = "g", FlexibilityType = "base" },
            new RecipeIngredient { Id = 12, RecipeId = 3, IngredientId = 18, Quantity = 100, Unit = "g", FlexibilityType = "base" },
            new RecipeIngredient { Id = 13, RecipeId = 3, IngredientId = 17, Quantity = 0.5m, Unit = "each", FlexibilityType = "base" },
            new RecipeIngredient { Id = 14, RecipeId = 3, IngredientId = 15, Quantity = 5, Unit = "ml", FlexibilityType = "base" },
            new RecipeIngredient { Id = 15, RecipeId = 3, IngredientId = 6, Quantity = 20, Unit = "g", FlexibilityType = "addition" },

            // Recipe 4: Lemon Herb Chicken with Brown Rice
            new RecipeIngredient { Id = 16, RecipeId = 4, IngredientId = 1, Quantity = 400, Unit = "g", FlexibilityType = "base" },
            new RecipeIngredient { Id = 17, RecipeId = 4, IngredientId = 13, Quantity = 200, Unit = "g", FlexibilityType = "base" },
            new RecipeIngredient { Id = 18, RecipeId = 4, IngredientId = 15, Quantity = 30, Unit = "ml", FlexibilityType = "base" },
            new RecipeIngredient { Id = 19, RecipeId = 4, IngredientId = 17, Quantity = 1, Unit = "each", FlexibilityType = "base" },
            new RecipeIngredient { Id = 20, RecipeId = 4, IngredientId = 12, Quantity = 10, Unit = "g", FlexibilityType = "base" },
            new RecipeIngredient { Id = 21, RecipeId = 4, IngredientId = 9, Quantity = 150, Unit = "g", FlexibilityType = "flexible", MinQuantity = 100, MaxQuantity = 250 },
            new RecipeIngredient { Id = 22, RecipeId = 4, IngredientId = 8, Quantity = 100, Unit = "g", FlexibilityType = "flexible", MinQuantity = 80, MaxQuantity = 200 },
            new RecipeIngredient { Id = 23, RecipeId = 4, IngredientId = 6, Quantity = 50, Unit = "g", FlexibilityType = "addition" },

            // Recipe 5: Mediterranean Lentil Soup
            new RecipeIngredient { Id = 24, RecipeId = 5, IngredientId = 3, Quantity = 300, Unit = "g", FlexibilityType = "base" },
            new RecipeIngredient { Id = 25, RecipeId = 5, IngredientId = 15, Quantity = 30, Unit = "ml", FlexibilityType = "base" },
            new RecipeIngredient { Id = 26, RecipeId = 5, IngredientId = 11, Quantity = 100, Unit = "g", FlexibilityType = "base" },
            new RecipeIngredient { Id = 27, RecipeId = 5, IngredientId = 12, Quantity = 10, Unit = "g", FlexibilityType = "base" },
            new RecipeIngredient { Id = 28, RecipeId = 5, IngredientId = 7, Quantity = 200, Unit = "g", FlexibilityType = "flexible", MinQuantity = 150, MaxQuantity = 300 },
            new RecipeIngredient { Id = 29, RecipeId = 5, IngredientId = 17, Quantity = 1, Unit = "each", FlexibilityType = "base" },
            new RecipeIngredient { Id = 30, RecipeId = 5, IngredientId = 6, Quantity = 80, Unit = "g", FlexibilityType = "flexible", MinQuantity = 50, MaxQuantity = 150 },
            new RecipeIngredient { Id = 31, RecipeId = 5, IngredientId = 10, Quantity = 60, Unit = "g", FlexibilityType = "addition" },

            // Recipe 6: Grilled Salmon with Roasted Vegetables
            new RecipeIngredient { Id = 32, RecipeId = 6, IngredientId = 2, Quantity = 400, Unit = "g", FlexibilityType = "base" },
            new RecipeIngredient { Id = 33, RecipeId = 6, IngredientId = 15, Quantity = 30, Unit = "ml", FlexibilityType = "base" },
            new RecipeIngredient { Id = 34, RecipeId = 6, IngredientId = 17, Quantity = 1, Unit = "each", FlexibilityType = "base" },
            new RecipeIngredient { Id = 35, RecipeId = 6, IngredientId = 12, Quantity = 10, Unit = "g", FlexibilityType = "base" },
            new RecipeIngredient { Id = 36, RecipeId = 6, IngredientId = 9, Quantity = 200, Unit = "g", FlexibilityType = "flexible", MinQuantity = 150, MaxQuantity = 300 },
            new RecipeIngredient { Id = 37, RecipeId = 6, IngredientId = 8, Quantity = 150, Unit = "g", FlexibilityType = "flexible", MinQuantity = 100, MaxQuantity = 250 },
            new RecipeIngredient { Id = 38, RecipeId = 6, IngredientId = 7, Quantity = 100, Unit = "g", FlexibilityType = "flexible", MinQuantity = 80, MaxQuantity = 200 },
            new RecipeIngredient { Id = 39, RecipeId = 6, IngredientId = 10, Quantity = 50, Unit = "g", FlexibilityType = "addition" },

            // Recipe 7: Chickpea and Vegetable Stir-Fry
            new RecipeIngredient { Id = 40, RecipeId = 7, IngredientId = 4, Quantity = 300, Unit = "g", FlexibilityType = "base" },
            new RecipeIngredient { Id = 41, RecipeId = 7, IngredientId = 15, Quantity = 20, Unit = "ml", FlexibilityType = "base" },
            new RecipeIngredient { Id = 42, RecipeId = 7, IngredientId = 11, Quantity = 80, Unit = "g", FlexibilityType = "base" },
            new RecipeIngredient { Id = 43, RecipeId = 7, IngredientId = 12, Quantity = 10, Unit = "g", FlexibilityType = "base" },
            new RecipeIngredient { Id = 44, RecipeId = 7, IngredientId = 8, Quantity = 150, Unit = "g", FlexibilityType = "flexible", MinQuantity = 100, MaxQuantity = 250 },
            new RecipeIngredient { Id = 45, RecipeId = 7, IngredientId = 9, Quantity = 150, Unit = "g", FlexibilityType = "flexible", MinQuantity = 100, MaxQuantity = 250 },
            new RecipeIngredient { Id = 46, RecipeId = 7, IngredientId = 7, Quantity = 100, Unit = "g", FlexibilityType = "flexible", MinQuantity = 80, MaxQuantity = 200 },
            new RecipeIngredient { Id = 47, RecipeId = 7, IngredientId = 13, Quantity = 150, Unit = "g", FlexibilityType = "base" },
            new RecipeIngredient { Id = 48, RecipeId = 7, IngredientId = 16, Quantity = 30, Unit = "g", FlexibilityType = "flexible", MinQuantity = 15, MaxQuantity = 50 },
            new RecipeIngredient { Id = 49, RecipeId = 7, IngredientId = 6, Quantity = 40, Unit = "g", FlexibilityType = "addition" },
            new RecipeIngredient { Id = 50, RecipeId = 7, IngredientId = 10, Quantity = 40, Unit = "g", FlexibilityType = "addition" },

            // Recipe 8: Hummus with Fresh Vegetables
            new RecipeIngredient { Id = 51, RecipeId = 8, IngredientId = 4, Quantity = 250, Unit = "g", FlexibilityType = "base" },
            new RecipeIngredient { Id = 52, RecipeId = 8, IngredientId = 15, Quantity = 30, Unit = "ml", FlexibilityType = "base" },
            new RecipeIngredient { Id = 53, RecipeId = 8, IngredientId = 17, Quantity = 1, Unit = "each", FlexibilityType = "base" },
            new RecipeIngredient { Id = 54, RecipeId = 8, IngredientId = 12, Quantity = 5, Unit = "g", FlexibilityType = "base" },
            new RecipeIngredient { Id = 55, RecipeId = 8, IngredientId = 8, Quantity = 100, Unit = "g", FlexibilityType = "flexible", MinQuantity = 80, MaxQuantity = 200 },
            new RecipeIngredient { Id = 56, RecipeId = 8, IngredientId = 9, Quantity = 80, Unit = "g", FlexibilityType = "flexible", MinQuantity = 60, MaxQuantity = 150 },
            new RecipeIngredient { Id = 57, RecipeId = 8, IngredientId = 7, Quantity = 80, Unit = "g", FlexibilityType = "flexible", MinQuantity = 60, MaxQuantity = 150 },
        };
    }
}
