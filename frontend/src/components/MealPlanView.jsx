import { useState } from 'react';
import DayCard from './DayCard';
import { regenerateMealPlan } from '../api';

const DAY_NAMES = [
  'Monday',
  'Tuesday',
  'Wednesday',
  'Thursday',
  'Friday',
  'Saturday',
  'Sunday',
];

export default function MealPlanView({
  mealPlan,
  members,
  onRegenerated,
  onFinalize,
  onBack,
}) {
  const [vetoes, setVetoes] = useState(new Set());
  const [vetoDetails, setVetoDetails] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const handleVeto = (dayIndex, dayName, mealType, meal) => {
    const key = `${dayName}-${mealType}`;
    setVetoes((prev) => new Set([...prev, key]));
    setVetoDetails((prev) => [
      ...prev,
      { dayIndex, mealType, recipeId: meal.recipeId },
    ]);
  };

  const handleRegenerate = async () => {
    setLoading(true);
    setError(null);
    try {
      const plan = await regenerateMealPlan(members, vetoDetails);
      onRegenerated(plan);
      setVetoes(new Set());
      setVetoDetails([]);
    } catch (err) {
      setError(
        err.response?.data?.message ||
        err.message ||
        'Failed to regenerate meal plan. Please try again.'
      );
    } finally {
      setLoading(false);
    }
  };

  const days = mealPlan.days || [];
  const snacks = mealPlan.snacks || [];

  return (
    <section className="step-section">
      <h2>Your Week</h2>
      <p className="step-description">
        Review your meal plan for the week. Veto any meals you don't want,
        then regenerate or finalize when you're happy.
      </p>

      {error && <div className="error-message">{error}</div>}

      {loading && (
        <div className="loading">
          <div className="spinner" />
          <p>Regenerating your meal plan...</p>
        </div>
      )}

      <div className="week-grid">
        {days.map((day, index) => (
          <DayCard
            key={DAY_NAMES[index]}
            day={day}
            dayIndex={index}
            dayName={DAY_NAMES[index]}
            vetoes={vetoes}
            onVeto={handleVeto}
          />
        ))}
      </div>

      {snacks.length > 0 && (
        <div className="snacks-section">
          <h3>Snacks for the Week</h3>
          <div className="snacks-list">
            {snacks.map((snack, index) => (
              <div key={index} className="snack-card">
                <h4>{snack.recipeName}</h4>
                {snack.prepTimeMinutes != null && (
                  <p className="meal-prep-time">{snack.prepTimeMinutes} min prep</p>
                )}
              </div>
            ))}
          </div>
        </div>
      )}

      <div className="step-actions">
        <button className="btn btn-outline" onClick={onBack} disabled={loading}>
          Back
        </button>
        <button
          className="btn btn-outline"
          onClick={handleRegenerate}
          disabled={vetoes.size === 0 || loading}
        >
          {loading ? 'Regenerating...' : 'Regenerate Plan'}
        </button>
        <button
          className="btn btn-primary"
          onClick={onFinalize}
          disabled={loading}
        >
          Finalize Plan
        </button>
      </div>
    </section>
  );
}
