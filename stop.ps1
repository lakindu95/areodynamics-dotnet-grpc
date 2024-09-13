Write-Output "Bringing down backend services..."
docker-compose -f ./aero-dynamics-backend/docker-compose.yml down
if ($LASTEXITCODE -ne 0) {
    Write-Output "Failed to bring down backend services!"
    exit $LASTEXITCODE
}

Write-Output "Bringing down frontend services..."
docker-compose -f ./aero-dynamics-frontend/docker-compose.yml down
if ($LASTEXITCODE -ne 0) {
    Write-Output "Failed to bring down frontend services!"
    exit $LASTEXITCODE
}

Write-Output "All services are down."
