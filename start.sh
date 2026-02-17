#!/bin/bash
set -e

echo "=== Kale - Starting up ==="
echo ""

# Start Postgres
echo "[1/4] Starting Postgres..."
docker compose up -d
echo "  Postgres running on localhost:5432"

# Wait for Postgres to be ready
echo "[2/4] Waiting for Postgres to accept connections..."
until docker compose exec -T db pg_isready -U kale > /dev/null 2>&1; do
  sleep 1
done
echo "  Postgres ready"

# Start backend
echo "[3/4] Starting backend..."
cd backend/Kale.Api
dotnet run &
BACKEND_PID=$!
cd ../..

# Wait for backend to be ready
until curl -sf http://localhost:5000/api/ingredients > /dev/null 2>&1; do
  sleep 1
done
echo "  Backend running on http://localhost:5000"

# Start frontend
echo "[4/4] Starting frontend..."
cd frontend
npm run dev &
FRONTEND_PID=$!
cd ..

sleep 2
echo ""
echo "=== Kale is ready! ==="
echo "  Frontend: http://localhost:5173"
echo "  Backend:  http://localhost:5000"
echo "  Swagger:  http://localhost:5000/swagger"
echo ""
echo "Press Ctrl+C to stop all services"

trap "kill $BACKEND_PID $FRONTEND_PID 2>/dev/null; docker compose stop; echo 'Stopped.'" EXIT
wait
