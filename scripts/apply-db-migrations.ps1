Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$apiProject = Join-Path $repoRoot "backend/src/SmartHome.Api/SmartHome.Api.csproj"

Write-Host "Applying SmartHome API database migrations..."

dotnet ef migrations list --project $apiProject --startup-project $apiProject | Out-Host
if ($LASTEXITCODE -ne 0) {
    throw "Failed to list migrations."
}

dotnet ef database update --project $apiProject --startup-project $apiProject | Out-Host
if ($LASTEXITCODE -ne 0) {
    throw "Failed to apply database migrations."
}

$pending = dotnet ef migrations list --project $apiProject --startup-project $apiProject | Select-String -Pattern "\(Pending\)"
if ($LASTEXITCODE -ne 0) {
    throw "Failed to verify migration state after update."
}
if ($pending) {
    throw "Migration application failed: pending migrations remain after update."
}

Write-Host "Database schema is current."
