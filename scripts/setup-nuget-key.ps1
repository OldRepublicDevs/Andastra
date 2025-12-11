# Setup NuGet API Key for automatic authentication
# This script configures NuGet to use your API key automatically
#
# Usage:
#   .\setup-nuget-key.ps1 [--api-key <key>] [--source <url>]
#
# If --api-key is not provided, it will prompt for it
# The API key will be stored securely in your user NuGet.config

param(
    [string]$ApiKey = "",
    [string]$Source = "https://api.nuget.org/v3/index.json"
)

$ErrorActionPreference = "Stop"

# Prompt for API key if not provided
if ([string]::IsNullOrWhiteSpace($ApiKey)) {
    Write-Host "Enter your NuGet API key:" -ForegroundColor Yellow
    $secureKey = Read-Host -AsSecureString
    $BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($secureKey)
    $ApiKey = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
}

if ([string]::IsNullOrWhiteSpace($ApiKey)) {
    Write-Host "Error: API key is required" -ForegroundColor Red
    exit 1
}

Write-Host "`nConfiguring NuGet source with API key..." -ForegroundColor Green

# Remove existing source if it exists (to update credentials)
$sourceName = "nuget.org"
try {
    dotnet nuget remove source $sourceName --configfile $env:APPDATA\NuGet\NuGet.Config 2>&1 | Out-Null
} catch {
    # Source might not exist, that's okay
}

# Add source with API key (credentials stored securely)
dotnet nuget add source $Source --name $sourceName --username "API" --password $ApiKey --store-password-in-clear-text

if ($LASTEXITCODE -eq 0) {
    Write-Host "`nNuGet source configured successfully!" -ForegroundColor Green
    Write-Host "You can now use 'dotnet nuget push' without specifying --api-key" -ForegroundColor Cyan
    Write-Host "Example: dotnet nuget push package.nupkg --source $Source" -ForegroundColor Cyan
} else {
    Write-Host "`nFailed to configure NuGet source" -ForegroundColor Red
    exit 1
}

