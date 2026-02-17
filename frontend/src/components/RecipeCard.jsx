import { useState } from 'react';

export default function RecipeCard({ recipe }) {
  const [expanded, setExpanded] = useState(false);

  return (
    <div className="recipe-card">
      <div
        className="recipe-card-header"
        onClick={() => setExpanded(!expanded)}
        role="button"
        tabIndex={0}
        onKeyDown={(e) => {
          if (e.key === 'Enter' || e.key === ' ') {
            e.preventDefault();
            setExpanded(!expanded);
          }
        }}
      >
        <h4>{recipe.recipeName}</h4>
        <div className="recipe-meta">
          {recipe.servings != null && <span>{recipe.servings} servings</span>}
          {recipe.prepTimeMinutes != null && (
            <span>{recipe.prepTimeMinutes} min</span>
          )}
          <span className="expand-icon">{expanded ? '\u25B2' : '\u25BC'}</span>
        </div>
      </div>

      {expanded && (
        <div className="recipe-card-body">
          {recipe.ingredients && recipe.ingredients.length > 0 && (
            <div className="recipe-ingredients">
              <h5>Ingredients</h5>
              <ul>
                {recipe.ingredients.map((ing, i) => (
                  <li key={i}>
                    {ing.quantity} {ing.unit} {ing.name}
                  </li>
                ))}
              </ul>
            </div>
          )}

          {recipe.instructions && (
            <div className="recipe-instructions">
              <h5>Instructions</h5>
              {Array.isArray(recipe.instructions) ? (
                <ol>
                  {recipe.instructions.map((step, i) => (
                    <li key={i}>{step}</li>
                  ))}
                </ol>
              ) : (
                <p>{recipe.instructions}</p>
              )}
            </div>
          )}

          {recipe.nutrientsPerServing && (
            <div className="recipe-nutrients">
              <h5>Nutrients per Serving</h5>
              <div className="nutrient-grid">
                {recipe.nutrientsPerServing.calories != null && (
                  <div className="nutrient-item">
                    <span className="nutrient-value">{recipe.nutrientsPerServing.calories}</span>
                    <span className="nutrient-label">Calories</span>
                  </div>
                )}
                {recipe.nutrientsPerServing.proteinG != null && (
                  <div className="nutrient-item">
                    <span className="nutrient-value">{recipe.nutrientsPerServing.proteinG}g</span>
                    <span className="nutrient-label">Protein</span>
                  </div>
                )}
                {recipe.nutrientsPerServing.carbsG != null && (
                  <div className="nutrient-item">
                    <span className="nutrient-value">{recipe.nutrientsPerServing.carbsG}g</span>
                    <span className="nutrient-label">Carbs</span>
                  </div>
                )}
                {recipe.nutrientsPerServing.fatG != null && (
                  <div className="nutrient-item">
                    <span className="nutrient-value">{recipe.nutrientsPerServing.fatG}g</span>
                    <span className="nutrient-label">Fat</span>
                  </div>
                )}
                {recipe.nutrientsPerServing.fiberG != null && (
                  <div className="nutrient-item">
                    <span className="nutrient-value">{recipe.nutrientsPerServing.fiberG}g</span>
                    <span className="nutrient-label">Fiber</span>
                  </div>
                )}
              </div>
            </div>
          )}
        </div>
      )}
    </div>
  );
}
