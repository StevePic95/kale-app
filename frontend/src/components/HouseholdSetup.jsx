import { useState } from 'react';
import MemberForm from './MemberForm';
import MemberCard from './MemberCard';
import { generateMealPlan } from '../api';

export default function HouseholdSetup({ members, setMembers, onPlanGenerated }) {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const handleAddMember = (member) => {
    setMembers((prev) => [...prev, member]);
  };

  const handleRemoveMember = (index) => {
    setMembers((prev) => prev.filter((_, i) => i !== index));
  };

  const handleGenerate = async () => {
    setLoading(true);
    setError(null);
    try {
      const plan = await generateMealPlan(members);
      onPlanGenerated(plan);
    } catch (err) {
      setError(
        err.response?.data?.message ||
        err.message ||
        'Failed to generate meal plan. Please try again.'
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <section className="step-section">
      <h2>Set Up Your Household</h2>
      <p className="step-description">
        Add everyone in your household so we can plan meals that meet
        each person's nutritional needs.
      </p>

      {members.length > 0 && (
        <div className="members-list">
          {members.map((member, index) => (
            <MemberCard
              key={index}
              member={member}
              onRemove={() => handleRemoveMember(index)}
            />
          ))}
        </div>
      )}

      <MemberForm onAdd={handleAddMember} />

      {error && <div className="error-message">{error}</div>}

      {loading && (
        <div className="loading">
          <div className="spinner" />
          <p>Generating your meal plan...</p>
        </div>
      )}

      <div className="step-actions">
        <button
          className="btn btn-primary"
          onClick={handleGenerate}
          disabled={members.length === 0 || loading}
        >
          {loading ? 'Generating...' : 'Generate Meal Plan'}
        </button>
      </div>
    </section>
  );
}
