export default function MealCard({ meal, mealType, isVetoed, onVeto }) {
  if (!meal) return null;

  const ingredientSummary = meal.ingredients
    ? meal.ingredients
        .slice(0, 4)
        .map((ing) => ing.name || ing)
        .join(', ') + (meal.ingredients.length > 4 ? '...' : '')
    : '';

  return (
    <div className={`meal-card ${isVetoed ? 'vetoed' : ''}`}>
      <div className="meal-card-header">
        <span className="meal-type-label">{mealType}</span>
        <button
          className={`btn btn-veto ${isVetoed ? 'vetoed' : ''}`}
          onClick={onVeto}
          disabled={isVetoed}
        >
          {isVetoed ? 'Vetoed' : 'Veto'}
        </button>
      </div>
      <h4 className={isVetoed ? 'strikethrough' : ''}>{meal.recipeName}</h4>
      {meal.prepTimeMinutes != null && (
        <p className="meal-prep-time">{meal.prepTimeMinutes} min prep</p>
      )}
      {ingredientSummary && (
        <p className="meal-ingredients-summary">{ingredientSummary}</p>
      )}
    </div>
  );
}
