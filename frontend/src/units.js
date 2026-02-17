const G_PER_OZ = 28.3495;
const G_PER_LB = 453.592;
const OZ_PER_LB = 16;

const ML_PER_TSP = 4.92892;
const ML_PER_TBSP = 14.7868;
const ML_PER_CUP = 236.588;

const CM_PER_INCH = 2.54;
const KG_PER_LB = 0.453592;

export function formatWeight(grams, system) {
  if (system === 'imperial') {
    const oz = grams / G_PER_OZ;
    if (oz >= OZ_PER_LB) {
      const lbs = grams / G_PER_LB;
      return { value: lbs, unit: 'lbs', display: `${round(lbs)} lbs` };
    }
    return { value: oz, unit: 'oz', display: `${round(oz)} oz` };
  }
  if (grams >= 1000) {
    const kg = grams / 1000;
    return { value: kg, unit: 'kg', display: `${round(kg)} kg` };
  }
  return { value: grams, unit: 'g', display: `${round(grams)} g` };
}

export function formatVolume(ml, system) {
  if (system === 'imperial') {
    const cups = ml / ML_PER_CUP;
    if (cups >= 0.25) {
      return { value: cups, unit: 'cups', display: `${round(cups)} cups` };
    }
    const tbsp = ml / ML_PER_TBSP;
    if (tbsp >= 1) {
      return { value: tbsp, unit: 'tbsp', display: `${round(tbsp)} tbsp` };
    }
    const tsp = ml / ML_PER_TSP;
    return { value: tsp, unit: 'tsp', display: `${round(tsp)} tsp` };
  }
  if (ml >= 1000) {
    const L = ml / 1000;
    return { value: L, unit: 'L', display: `${round(L)} L` };
  }
  return { value: ml, unit: 'ml', display: `${round(ml)} ml` };
}

export function formatIngredient(quantity, unit, system) {
  if (unit === 'g') return formatWeight(quantity, system);
  if (unit === 'ml') return formatVolume(quantity, system);
  return { value: quantity, unit, display: `${round(quantity)} ${unit}` };
}

export function formatHeight(cm, system) {
  if (system === 'imperial') {
    const totalInches = Math.round(cm / CM_PER_INCH);
    const feet = Math.floor(totalInches / 12);
    const inches = totalInches % 12;
    return `${feet}'${inches}"`;
  }
  return `${round(cm)} cm`;
}

export function formatBodyWeight(kg, system) {
  if (system === 'imperial') {
    const lbs = kg / KG_PER_LB;
    return `${round(lbs)} lbs`;
  }
  return `${round(kg)} kg`;
}

export function heightToCm(value, system) {
  if (system === 'imperial') {
    return value * CM_PER_INCH;
  }
  return value;
}

export function weightToKg(value, system) {
  if (system === 'imperial') {
    return value * KG_PER_LB;
  }
  return value;
}

function round(n) {
  if (n >= 100) return Math.round(n);
  if (n >= 10) return Math.round(n * 10) / 10;
  return Math.round(n * 100) / 100;
}
