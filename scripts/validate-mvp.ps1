Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Write-Host "Validating Smart Home MVP quickstart..."
Write-Host "1) Ensure backend is running on http://localhost:5151"
Write-Host "2) Ensure frontend is running on http://localhost:4200"
Write-Host "3) Ensure simulator is posting telemetry"
Write-Host "4) Verify /api/devices and /api/telemetry/latest endpoints respond"

$repoRoot = Split-Path -Parent $PSScriptRoot
$apiProject = Join-Path $repoRoot "backend/src/SmartHome.Api/SmartHome.Api.csproj"

Write-Host "5) Running schema synchronization check..."
dotnet ef database update --project $apiProject --startup-project $apiProject | Out-Host
if ($LASTEXITCODE -ne 0) {
	throw "Schema synchronization failed during validate-mvp."
}

$migrationsOutput = dotnet ef migrations list --project $apiProject --startup-project $apiProject
$lastCommandExitCode = $LASTEXITCODE
$migrationsOutput | Out-Host
if ($lastCommandExitCode -ne 0) {
	throw "Failed to query migration state during validate-mvp."
}
$pendingLines = $migrationsOutput | Select-String -Pattern "\(Pending\)"
if ($pendingLines) {
	throw "Schema validation failed: pending migrations remain after update."
}

if ($env:CI) {
	Write-Host "CI mode detected: schema synchronization validation passed with no pending migrations."
}
