import MealCard from './MealCard';

export default function DayCard({ day, dayIndex, dayName, vetoes, onVeto }) {
  return (
    <div className="day-card">
      <h3 className="day-name">{dayName}</h3>
      <div className="day-meals">
        {day.breakfast && (
          <MealCard
            meal={day.breakfast}
            mealType="Breakfast"
            isVetoed={vetoes.has(`${dayName}-breakfast`)}
            onVeto={() => onVeto(dayIndex, dayName, 'breakfast', day.breakfast)}
          />
        )}
        {day.dinner && (
          <MealCard
            meal={day.dinner}
            mealType="Dinner"
            isVetoed={vetoes.has(`${dayName}-dinner`)}
            onVeto={() => onVeto(dayIndex, dayName, 'dinner', day.dinner)}
          />
        )}
      </div>
    </div>
  );
}
