# Kale

An app that helps users coordinate their grocery shopping and meal plan, optimizing for price, healthiness, ease, and deliciousness.

> **Note:** "Kale" is a working name and may change.

## Concept

Users provide details on each member of their household and their dietary needs, including:

- Height, weight, sex, activity level, goals, etc. — everything needed to determine daily calorie and nutrient goals
- Allergies
- Likes
- Dislikes

The app plans out meals for everyone for one week. Users can veto meals they don't want, and the app will recalculate the week with new meals while ensuring everyone still hits their required nutrients. The final output is a clean, minimal PDF or HTML page with a shopping list and all the recipes for the week.

The intended workflow: sign up on Sunday, input your household information, review and finalize the meal plan, get your PDF, and go shopping. Throughout the week, use the recipes to cook for your household. Next Sunday, do it again. Nothing goes bad, your grocery bill goes down, the household is eating better than ever, and everyone's diet is extremely healthy.

## Philosophy

The app is opinionated. It follows the **Mediterranean Diet** and always yields dinners portioned so that everyone has leftovers for lunch the next day. The app tells the user what to eat and when — the veto mechanism exists only as an escape hatch for meals a user really doesn't want.

### Meal Coverage

- **Dinners** — scheduled, portioned for leftover lunches the next day
- **Breakfasts** — scheduled
- **Snacks** — included as recipes and ingredients but unscheduled, used at the user's discretion

### Households

The app covers a single household. There is no artificial cap on household members — the practical limit is the user's kitchen capacity.

## Data

### Ingredient Database

Uses **USDA FoodData Central** for the MVP, with the goal of maintaining an independent database eventually. The database should have very detailed nutritional data for all ingredients you'd expect to find in a grocery store.

Ingredient cost data uses **rough national averages** for the MVP.

### Recipe Database

Recipes are **hand-curated** (~30-50 for MVP) with a flexible structure that allows the meal planning algorithm to modify them to hit nutritional targets. Each recipe has well-structured information about the ingredients that go into it and come out of it (for instance, chicken noodle soup requires raw carrots to cook, but when you eat it, those carrots are cooked and have a different micronutrient profile).

#### Flexible Recipe Model

Recipes are modeled in three layers:

1. **Base ingredients** — the core identity of the dish. These have fixed or minimum quantities. Chicken noodle soup needs chicken, noodles, broth, and basic aromatics.
2. **Flexible ingredients** — ingredients that can be scaled up or down within a defined range without changing the dish's character. E.g., carrots in the soup could be 100-200g.
3. **Compatible additions** — ingredients that can be injected into the recipe to fill nutritional gaps. Compatibility is governed by tags (dish type like `soup`, `stir-fry`, `salad`, etc.) so that additions make culinary sense. Kale can go in a soup; kale does not go in a creme brulee.

## Meal Planning Algorithm

The meal planner is a **deterministic algorithm** (no AI/LLM involvement). It takes as input household profiles, the recipe pool, ingredient costs, and nutritional targets, then optimizes a full week of meals.

### Constraints

- Allergies (hard constraint)
- Dislikes (hard constraint)
- Macro and micronutrient targets per person per day (soft constraint — some wiggle room is acceptable)

### Optimization Goals

- Meet everyone's micro and macronutrient needs as closely as possible, according to proven nutritional scientific consensus
- Maximize meals everyone enjoys
- Keep recipes reasonably easy to cook
- Portion properly — minimize food waste
- Minimize cost of groceries
- Maintain variety across the week

### Three Levers

The algorithm has three levers to achieve these goals:

1. **Choose which recipes** to schedule across the week
2. **Scale flexible ingredients** within their allowed ranges
3. **Inject compatible additions** to fill nutritional gaps

## User Experience

The app is opinionated and requires minimal configuration. The user inputs their household, reviews a generated plan, optionally vetoes meals, and exports the result. The interaction should be fast — not a 30-minute planning session.

When a meal is vetoed, the algorithm recalculates the rest of the week to ensure nutritional targets are still met.

## Output

A clean, minimal PDF or HTML page containing:

- A consolidated **shopping list** for the week
- All **recipes** for the week with final adjusted quantities

## Tech Stack (MVP)

| Layer | Choice |
|---|---|
| **Frontend** | React (chosen for future React Native transition to mobile) |
| **Backend** | .NET / C# |
| **ORM** | Entity Framework Core |
| **Database** | PostgreSQL |
| **Auth** | None for MVP (local/session-based) |

### Deployment (MVP)

- **Frontend** — GitHub Pages or Vercel (free)
- **Backend** — Azure App Service Free Tier or Railway free tier
- **Database** — Free-tier Postgres provider (Neon, Supabase, etc.)

## Local Development

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (v18+)
- [Docker](https://www.docker.com/) (for Postgres)

### Quick Start

```bash
./start.sh
```

This starts Postgres, the backend (with auto-migration and seed data), and the frontend dev server. The app will be available at **http://localhost:5173**.

### Manual Start

```bash
# 1. Start Postgres
docker compose up -d

# 2. Start the backend (applies migrations and seeds data automatically)
cd backend/Kale.Api
dotnet run

# 3. In another terminal, start the frontend
cd frontend
npm install
npm run dev
```

- Frontend: http://localhost:5173
- Backend API: http://localhost:5000
- Swagger UI: http://localhost:5000/swagger

## Future Plans

- Independent ingredient database (beyond USDA)
- User accounts and authentication
- Mobile app via React Native
- Regional/store-specific grocery pricing
