# Installs dependencies and builds everything (frontend apps, then backend).
# Usage: .\scripts\build-all.ps1 [-Configuration Release] [-Frozen]
param(
    [string]$Configuration = 'Release',
    [switch]$Frozen  # use --frozen-lockfile (as CI does)
)
$ErrorActionPreference = 'Stop'
Set-Location (Split-Path $PSScriptRoot -Parent)

function Run($cmd) {
    Write-Host "> $cmd" -ForegroundColor Cyan
    Invoke-Expression $cmd
    if ($LASTEXITCODE -ne 0) { throw "Failed: $cmd" }
}

Run ('pnpm install' + $(if ($Frozen) { ' --frozen-lockfile' } else { '' }))
Run 'pnpm -r typecheck'
Run 'pnpm -r build'
Run 'dotnet restore SentryPlayground.Web/SentryPlayground.Web.csproj'
Run "dotnet build SentryPlayground.Web/SentryPlayground.Web.csproj -c $Configuration --no-restore"
Write-Host 'Build complete.' -ForegroundColor Green
