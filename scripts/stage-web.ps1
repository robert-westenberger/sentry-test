# Copies the runnable web site (pages, config, static assets, bin) into artifacts\web.
# Run after the frontend and backend builds.
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$src  = Join-Path $root 'SentryPlayground.Web'
$dest = Join-Path $root 'artifacts\web'

if (Test-Path $dest) { Remove-Item $dest -Recurse -Force }
New-Item -ItemType Directory -Path $dest | Out-Null

foreach ($pattern in '*.aspx', '*.ashx', 'Global.asax', 'Web.config') {
    Copy-Item (Join-Path $src $pattern) $dest
}
Copy-Item (Join-Path $src 'static') $dest -Recurse
Copy-Item (Join-Path $src 'bin') $dest -Recurse
Write-Host "Staged web site at $dest"
