import { useState } from 'react';
import HouseholdSetup from './components/HouseholdSetup';
import MealPlanView from './components/MealPlanView';
import PlanOutput from './components/PlanOutput';
import SettingsPanel from './components/SettingsPanel';

function App() {
  const [step, setStep] = useState(1);
  const [members, setMembers] = useState([]);
  const [mealPlan, setMealPlan] = useState(null);

  const handlePlanGenerated = (plan) => {
    setMealPlan(plan);
    setStep(2);
  };

  const handlePlanRegenerated = (plan) => {
    setMealPlan(plan);
  };

  const handleFinalize = () => {
    setStep(3);
  };

  const handleBack = () => {
    setStep(step - 1);
  };

  const handleStartOver = () => {
    setMembers([]);
    setMealPlan(null);
    setStep(1);
  };

  return (
    <div className="app">
      <header className="app-header">
        <h1 className="app-logo">Kale</h1>
        <div className="step-indicator">
          <span className={`step-dot ${step >= 1 ? 'active' : ''}`}>1</span>
          <span className="step-line" />
          <span className={`step-dot ${step >= 2 ? 'active' : ''}`}>2</span>
          <span className="step-line" />
          <span className={`step-dot ${step >= 3 ? 'active' : ''}`}>3</span>
        </div>
        <SettingsPanel />
      </header>

      <main className="app-main">
        {step === 1 && (
          <HouseholdSetup
            members={members}
            setMembers={setMembers}
            onPlanGenerated={handlePlanGenerated}
          />
        )}
        {step === 2 && mealPlan && (
          <MealPlanView
            mealPlan={mealPlan}
            members={members}
            onRegenerated={handlePlanRegenerated}
            onFinalize={handleFinalize}
            onBack={handleBack}
          />
        )}
        {step === 3 && mealPlan && (
          <PlanOutput
            mealPlan={mealPlan}
            onStartOver={handleStartOver}
          />
        )}
      </main>

      <footer className="app-footer no-print">
        <p>Kale — Eat well, spend less.</p>
      </footer>
    </div>
  );
}

export default App;
