# start-alci-docker.ps1

Clear-Host
Write-Host "[SYSTEM] ALCI Order Stream: INITIALIZING DOCKER INFRASTRUCTURE" -ForegroundColor Cyan
Write-Host "------------------------------------------------------------" -ForegroundColor Gray

# 1. Cleanup Phase (Termination Protocol)
Write-Host "[1/3] CLEANUP: Stopping legacy containers and clearing volumes..." -ForegroundColor Yellow
docker-compose down -v

# 2. Reconstruction Phase (The Collision)
Write-Host "[2/3] BUILD: Building images and spinning up services (8080/8081)..." -ForegroundColor Magenta
docker-compose up -d --build

# 3. Quick Health Check
Write-Host "------------------------------------------------------------" -ForegroundColor Gray
Write-Host "[SUCCESS] ALCI Order Stream IS ONLINE" -ForegroundColor Green
Write-Host ">> Client:  http://localhost:8080" -ForegroundColor White
Write-Host ">> Swagger: http://localhost:8081/swagger" -ForegroundColor White
Write-Host "------------------------------------------------------------" -ForegroundColor Gray
Write-Host "TIP: Run 'docker-compose logs -f' to monitor real-time updates." -ForegroundColor DarkGray