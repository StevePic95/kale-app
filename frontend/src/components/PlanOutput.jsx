import ShoppingList from './ShoppingList';
import RecipeCard from './RecipeCard';

export default function PlanOutput({ mealPlan, onStartOver }) {
  const shoppingList = mealPlan.shoppingList || [];

  const recipes = [];
  const seenRecipeIds = new Set();
  for (const day of mealPlan.days || []) {
    for (const meal of [day.breakfast, day.dinner]) {
      if (meal && meal.recipeId && !seenRecipeIds.has(meal.recipeId)) {
        seenRecipeIds.add(meal.recipeId);
        recipes.push(meal);
      }
    }
  }
  for (const snack of mealPlan.snacks || []) {
    if (snack && snack.recipeId && !seenRecipeIds.has(snack.recipeId)) {
      seenRecipeIds.add(snack.recipeId);
      recipes.push(snack);
    }
  }

  const handlePrint = () => {
    window.print();
  };

  return (
    <section className="step-section">
      <h2>Your Shopping List &amp; Recipes</h2>
      <p className="step-description">
        Here is everything you need for the week. Print or save this page
        for reference while shopping and cooking.
      </p>

      <ShoppingList shoppingList={shoppingList} />

      {recipes.length > 0 && (
        <div className="recipes-section">
          <h3>Recipes</h3>
          <div className="recipes-list">
            {recipes.map((recipe, index) => (
              <RecipeCard key={recipe.recipeId || index} recipe={recipe} />
            ))}
          </div>
        </div>
      )}

      <div className="step-actions no-print">
        <button className="btn btn-outline" onClick={onStartOver}>
          Start Over
        </button>
        <button className="btn btn-primary" onClick={handlePrint}>
          Print / Save
        </button>
      </div>
    </section>
  );
}
