import { useState, useRef, useEffect } from 'react';
import { useSettings } from '../context/SettingsContext';

export default function SettingsPanel() {
  const { units, setUnits, theme, setTheme } = useSettings();
  const [open, setOpen] = useState(false);
  const panelRef = useRef(null);

  useEffect(() => {
    function handleClickOutside(e) {
      if (panelRef.current && !panelRef.current.contains(e.target)) {
        setOpen(false);
      }
    }
    function handleEscape(e) {
      if (e.key === 'Escape') setOpen(false);
    }
    if (open) {
      document.addEventListener('mousedown', handleClickOutside);
      document.addEventListener('keydown', handleEscape);
    }
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
      document.removeEventListener('keydown', handleEscape);
    };
  }, [open]);

  return (
    <div className="settings-panel" ref={panelRef}>
      <button
        className="settings-toggle"
        onClick={() => setOpen(!open)}
        aria-label="Settings"
        title="Settings"
      >
        &#9881;
      </button>
      {open && (
        <div className="settings-dropdown">
          <div className="settings-group">
            <label className="settings-label">Units</label>
            <div className="settings-toggle-group">
              <button
                className={`settings-option ${units === 'metric' ? 'active' : ''}`}
                onClick={() => setUnits('metric')}
              >
                Metric
              </button>
              <button
                className={`settings-option ${units === 'imperial' ? 'active' : ''}`}
                onClick={() => setUnits('imperial')}
              >
                Imperial
              </button>
            </div>
          </div>
          <div className="settings-group">
            <label className="settings-label">Theme</label>
            <div className="settings-toggle-group">
              <button
                className={`settings-option ${theme === 'light' ? 'active' : ''}`}
                onClick={() => setTheme('light')}
              >
                Light
              </button>
              <button
                className={`settings-option ${theme === 'dark' ? 'active' : ''}`}
                onClick={() => setTheme('dark')}
              >
                Dark
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
