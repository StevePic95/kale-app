import { useState } from 'react';
import { useSettings } from '../context/SettingsContext';
import { heightToCm, weightToKg } from '../units';

const ACTIVITY_LEVELS = [
  { value: 'sedentary', label: 'Sedentary' },
  { value: 'light', label: 'Light' },
  { value: 'moderate', label: 'Moderate' },
  { value: 'active', label: 'Active' },
];

const emptyMember = {
  name: '',
  age: '',
  sex: 'female',
  heightCm: '',
  heightFt: '',
  heightIn: '',
  weightKg: '',
  activityLevel: 'moderate',
  allergies: '',
  likes: '',
  dislikes: '',
};

export default function MemberForm({ onAdd }) {
  const { units } = useSettings();
  const [form, setForm] = useState({ ...emptyMember });
  const isImperial = units === 'imperial';

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!form.name.trim()) return;

    let heightCm, weightKg;
    if (isImperial) {
      const feet = parseInt(form.heightFt, 10) || 0;
      const inches = parseInt(form.heightIn, 10) || 0;
      heightCm = heightToCm(feet * 12 + inches, 'imperial');
      weightKg = weightToKg(parseFloat(form.weightKg) || 0, 'imperial');
    } else {
      heightCm = parseFloat(form.heightCm) || 0;
      weightKg = parseFloat(form.weightKg) || 0;
    }

    const member = {
      name: form.name.trim(),
      age: parseInt(form.age, 10) || 0,
      sex: form.sex,
      heightCm,
      weightKg,
      activityLevel: form.activityLevel,
      allergies: form.allergies
        .split(',')
        .map((s) => s.trim())
        .filter(Boolean),
      likes: form.likes
        .split(',')
        .map((s) => s.trim())
        .filter(Boolean),
      dislikes: form.dislikes
        .split(',')
        .map((s) => s.trim())
        .filter(Boolean),
    };

    onAdd(member);
    setForm({ ...emptyMember });
  };

  return (
    <form className="member-form" onSubmit={handleSubmit}>
      <h3>Add a Household Member</h3>

      <div className="form-row">
        <div className="form-group">
          <label htmlFor="name">Name</label>
          <input
            id="name"
            name="name"
            type="text"
            placeholder="e.g. Alex"
            value={form.name}
            onChange={handleChange}
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="age">Age</label>
          <input
            id="age"
            name="age"
            type="number"
            min="1"
            max="120"
            placeholder="30"
            value={form.age}
            onChange={handleChange}
            required
          />
        </div>
      </div>

      <div className="form-row">
        <div className="form-group">
          <label htmlFor="sex">Sex</label>
          <select id="sex" name="sex" value={form.sex} onChange={handleChange}>
            <option value="female">Female</option>
            <option value="male">Male</option>
          </select>
        </div>
        <div className="form-group">
          <label htmlFor="activityLevel">Activity Level</label>
          <select
            id="activityLevel"
            name="activityLevel"
            value={form.activityLevel}
            onChange={handleChange}
          >
            {ACTIVITY_LEVELS.map((level) => (
              <option key={level.value} value={level.value}>
                {level.label}
              </option>
            ))}
          </select>
        </div>
      </div>

      <div className="form-row">
        {isImperial ? (
          <div className="form-group">
            <label>Height</label>
            <div style={{ display: 'flex', gap: '0.5rem' }}>
              <input
                name="heightFt"
                type="number"
                min="0"
                max="8"
                placeholder="ft"
                value={form.heightFt}
                onChange={handleChange}
                required
                style={{ flex: 1 }}
              />
              <input
                name="heightIn"
                type="number"
                min="0"
                max="11"
                placeholder="in"
                value={form.heightIn}
                onChange={handleChange}
                required
                style={{ flex: 1 }}
              />
            </div>
          </div>
        ) : (
          <div className="form-group">
            <label htmlFor="heightCm">Height (cm)</label>
            <input
              id="heightCm"
              name="heightCm"
              type="number"
              min="50"
              max="250"
              placeholder="170"
              value={form.heightCm}
              onChange={handleChange}
              required
            />
          </div>
        )}
        <div className="form-group">
          <label htmlFor="weightKg">
            {isImperial ? 'Weight (lbs)' : 'Weight (kg)'}
          </label>
          <input
            id="weightKg"
            name="weightKg"
            type="number"
            min={isImperial ? '20' : '10'}
            max={isImperial ? '660' : '300'}
            placeholder={isImperial ? '154' : '70'}
            value={form.weightKg}
            onChange={handleChange}
            required
          />
        </div>
      </div>

      <div className="form-group full-width">
        <label htmlFor="allergies">Allergies (comma-separated)</label>
        <input
          id="allergies"
          name="allergies"
          type="text"
          placeholder="e.g. peanuts, shellfish"
          value={form.allergies}
          onChange={handleChange}
        />
      </div>

      <div className="form-group full-width">
        <label htmlFor="likes">Likes (comma-separated)</label>
        <input
          id="likes"
          name="likes"
          type="text"
          placeholder="e.g. salmon, avocado, pasta"
          value={form.likes}
          onChange={handleChange}
        />
      </div>

      <div className="form-group full-width">
        <label htmlFor="dislikes">Dislikes (comma-separated)</label>
        <input
          id="dislikes"
          name="dislikes"
          type="text"
          placeholder="e.g. olives, anchovies"
          value={form.dislikes}
          onChange={handleChange}
        />
      </div>

      <button type="submit" className="btn btn-secondary">
        Add Member
      </button>
    </form>
  );
}
