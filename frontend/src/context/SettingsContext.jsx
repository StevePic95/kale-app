import { createContext, useContext, useState, useEffect } from 'react';

const SettingsContext = createContext();

function getInitial(key, fallback) {
  try {
    const stored = localStorage.getItem(key);
    return stored || fallback;
  } catch {
    return fallback;
  }
}

export function SettingsProvider({ children }) {
  const [units, setUnits] = useState(() => getInitial('kale-units', 'metric'));
  const [theme, setTheme] = useState(() => getInitial('kale-theme', 'light'));

  useEffect(() => {
    localStorage.setItem('kale-units', units);
  }, [units]);

  useEffect(() => {
    localStorage.setItem('kale-theme', theme);
    document.documentElement.dataset.theme = theme;
  }, [theme]);

  useEffect(() => {
    document.documentElement.dataset.theme = theme;
  }, []);

  return (
    <SettingsContext.Provider value={{ units, setUnits, theme, setTheme }}>
      {children}
    </SettingsContext.Provider>
  );
}

export function useSettings() {
  const ctx = useContext(SettingsContext);
  if (!ctx) throw new Error('useSettings must be used within SettingsProvider');
  return ctx;
}
