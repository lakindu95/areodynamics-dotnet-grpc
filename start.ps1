Write-Output "Building backend services..."
docker-compose -f ./aero-dynamics-backend/docker-compose.yml build
if ($LASTEXITCODE -ne 0) {
    Write-Output "Backend services build failed!"
    exit $LASTEXITCODE
}

Write-Output "Building frontend services..."
docker-compose -f ./aero-dynamics-frontend/docker-compose.yml build
if ($LASTEXITCODE -ne 0) {
    Write-Output "Frontend services build failed!"
    exit $LASTEXITCODE
}

Write-Output "Bringing up backend services..."
Set-Location -Path ".\aero-dynamics-backend"
docker-compose up -d
if ($LASTEXITCODE -ne 0) {
    Write-Output "Failed to bring up backend services!"
    exit $LASTEXITCODE
}

Set-Location -Path ".."
Write-Output "Bringing up frontend services..."
Set-Location -Path ".\aero-dynamics-frontend"
docker-compose up -d
if ($LASTEXITCODE -ne 0) {
    Write-Output "Failed to bring up frontend services!"
    exit $LASTEXITCODE
}
Set-Location -Path ".."
Write-Output "All services are up and running."
