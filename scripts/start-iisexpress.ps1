# Hosts a site folder with IIS Express. Defaults to the web project folder (after a Release build).
# Usage: .\scripts\start-iisexpress.ps1 [-Path <site folder>] [-Port 8080]
param(
    [string]$Path = (Join-Path (Split-Path $PSScriptRoot -Parent) 'SentryPlayground.Web'),
    [int]$Port = 8080
)
$ErrorActionPreference = 'Stop'
$exe = @("$env:ProgramFiles\IIS Express\iisexpress.exe", "${env:ProgramFiles(x86)}\IIS Express\iisexpress.exe") |
    Where-Object { Test-Path $_ } | Select-Object -First 1
if (-not $exe) { throw 'IIS Express not found. Install it first (see README).' }
$Path = (Resolve-Path $Path).Path
if (-not (Test-Path (Join-Path $Path 'bin\SentryPlayground.Web.dll'))) { throw "No bin\SentryPlayground.Web.dll in $Path - build the backend first." }
Write-Host "http://localhost:$Port/app-one.aspx  (Ctrl+C to stop)"
& $exe /path:$Path /port:$Port
