import axios from 'axios';

const API_BASE = 'http://localhost:5000';

const client = axios.create({
  baseURL: API_BASE,
  headers: {
    'Content-Type': 'application/json',
  },
});

export async function generateMealPlan(members) {
  const response = await client.post('/api/mealplan/generate', { members });
  return response.data;
}

export async function regenerateMealPlan(members, vetoes) {
  const response = await client.post('/api/mealplan/regenerate', { members, vetoes });
  return response.data;
}

export async function getIngredients() {
  const response = await client.get('/api/ingredients');
  return response.data;
}

export async function getRecipes() {
  const response = await client.get('/api/recipes');
  return response.data;
}
