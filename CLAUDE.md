# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What This Is

Kale is a full-stack meal planning app (Mediterranean Diet, opinionated) with a .NET 9 backend, React 19 frontend, and PostgreSQL database. See README.md for the full product spec.

## Commands

### Running Locally

```bash
# Start Postgres (required first)
docker compose up -d

# Backend (auto-migrates and seeds on startup)
cd backend/Kale.Api && dotnet run    # http://localhost:5000

# Frontend (separate terminal)
cd frontend && npm install && npm run dev    # http://localhost:5173

# Or all at once:
./start.sh
```

### Building

```bash
# Backend
cd backend/Kale.Api && dotnet build

# Frontend
cd frontend && npm run build
```

### Linting

```bash
cd frontend && npm run lint
```

### EF Core Migrations

```bash
cd backend/Kale.Api
dotnet-ef migrations add <MigrationName>
dotnet-ef migrations remove    # undo last migration
```

Migrations are auto-applied on backend startup via `Database.Migrate()` in Program.cs.

### Database Reset

```bash
docker compose exec -T db psql -U kale -c "DROP SCHEMA public CASCADE; CREATE SCHEMA public;"
# Then restart backend to re-migrate and re-seed
```

## Architecture

### Backend (backend/Kale.Api/)

ASP.NET Core Web API. No authentication in MVP — stateless request/response.

- **Controllers/** — REST endpoints: `POST /api/mealplan/generate`, `POST /api/mealplan/regenerate`, `GET /api/ingredients`, `GET /api/recipes`
- **Services/MealPlannerService.cs** — The core algorithm. Deterministic (no AI/randomness). Calculates BMR via Mifflin-St Jeor, filters by allergies/dislikes, selects recipes for 7 days, scales ingredients, generates shopping list with costs. This is the most complex file in the codebase.
- **Models/** — EF Core entities: `Ingredient` (nutritional data per 100g), `Recipe`, `RecipeIngredient` (links with flexibility types: "base"/"flexible"/"addition")
- **Data/SeedData.cs** — 19 ingredients, 8 recipes, 57 recipe-ingredient links. Resets Postgres identity sequences after seeding.
- **Dtos/MealPlanDtos.cs** — All request/response DTOs in a single file

Key conventions: decimal(10,2) precision for all nutritional/cost values, camelCase JSON serialization, CORS configured for localhost:5173.

### Frontend (frontend/)

React 19 + Vite. Single-page app with a 3-step wizard managed by state in App.jsx (no router).

- **Step 1 (HouseholdSetup)** — Add household members with dietary profiles
- **Step 2 (MealPlanView)** — Review 7-day plan, veto meals, regenerate
- **Step 3 (PlanOutput)** — Shopping list + recipes for export/print

API client in `src/api.js` talks to backend at `http://localhost:5000`.

Settings (metric/imperial units, light/dark theme) are managed via `SettingsContext` in `src/context/SettingsContext.jsx`, persisted to localStorage. Unit conversion utilities live in `src/units.js` — the backend always works in metric, and the frontend converts for display and back before API requests. Dark mode uses `[data-theme="dark"]` CSS custom property overrides on `<html>`.

### Data Flow

Household members → `POST /api/mealplan/generate` → algorithm filters recipes by allergies, calculates per-person calorie targets, picks diverse meals for the week (dinners portioned 2x for leftover lunches), scales flexible ingredients, aggregates shopping list → returns full plan. Vetoes trigger `POST /api/mealplan/regenerate` which excludes vetoed recipes and recalculates.

### Flexible Recipe Model

Recipes have 3 ingredient tiers that the algorithm uses to adjust nutritional output:
- **base** — fixed, defines the dish identity
- **flexible** — has min/max quantity range the algorithm can adjust
- **addition** — optional ingredients injected to fill nutrient gaps (governed by dish tags)

## Configuration

| Service | Port | Config |
|---------|------|--------|
| Frontend | 5173 | frontend/vite.config.js |
| Backend | 5000 | backend/Kale.Api/Properties/launchSettings.json |
| Postgres | 5432 | docker-compose.yml (user: kale, pass: kale_dev, db: kale) |

Connection string is in `backend/Kale.Api/appsettings.json`.
