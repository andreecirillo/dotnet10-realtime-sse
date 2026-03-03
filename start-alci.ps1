# start-alci.ps1

# 1. Full cleanup of previous dotnet processes to ensure a fresh environment for ALCI.OrderStream apps
taskkill /F /IM dotnet.exe 2>$null

$apiProj = "apps/ALCI.OrderStream.Api/ALCI.OrderStream.Api.csproj"
$clientProj = "apps/ALCI.OrderStream.Client/ALCI.OrderStream.Client.csproj"

# 2. Start the ALCI.OrderStream.API in a new terminal
Write-Host "Starting ALCI.OrderStream.API..." -ForegroundColor Cyan
$apiProcess = Start-Process dotnet -ArgumentList "run", "--project", "$apiProj", "--launch-profile", "https" -PassThru

# 3. Grace period + FORCE SWAGGER UI LAUNCH
# Manually launching the browser as 'dotnet run' via script may bypass 'launchBrowser'
Write-Host "Waiting for API initialization and launching Swagger..." -ForegroundColor Gray
Start-Sleep -Seconds 5
Start-Process "https://localhost:7217/swagger"

# 4. Start the ALCI.OrderStream.Client (watch mode handles browser launch)
Write-Host "Starting ALCI.OrderStream.Client..." -ForegroundColor Green
try {
    dotnet watch --project $clientProj --launch-profile "https"
}
# 5. ALCI.OrderStream Termination Protocol
finally {
    Write-Host "Shutting down ALCI.OrderStream apps..." -ForegroundColor Yellow
    taskkill /F /IM dotnet.exe 2>$null
    if ($apiProcess -and -not $apiProcess.HasExited) {
        Stop-Process -Id $apiProcess.Id -Force 2>$null
    }
}