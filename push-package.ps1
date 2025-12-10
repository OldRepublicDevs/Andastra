$ErrorActionPreference = "Stop"

# Get API key from .env
$envContent = Get-Content ".env" -Raw
if ($envContent -match 'NUGET_API_KEY\s*=\s*"([^"]+)"') {
    $apiKey = $matches[1]
    Write-Host "API key loaded from .env" -ForegroundColor Green
} else {
    Write-Host "ERROR: Could not find NUGET_API_KEY in .env file" -ForegroundColor Red
    exit 1
}

# Find package
$pkg = Get-ChildItem "src/CSharpKOTOR/bin/Release" -Filter "*.nupkg" | Sort-Object LastWriteTime -Descending | Select-Object -First 1
if (-not $pkg) {
    Write-Host "ERROR: No package found!" -ForegroundColor Red
    exit 1
}

Write-Host "Found package: $($pkg.Name)" -ForegroundColor Cyan

# Check package metadata
Add-Type -AssemblyName System.IO.Compression.FileSystem
$zip = [System.IO.Compression.ZipFile]::OpenRead($pkg.FullName)
$nuspec = $zip.Entries | Where-Object { $_.FullName -like "*.nuspec" } | Select-Object -First 1
$stream = $nuspec.Open()
$reader = New-Object System.IO.StreamReader($stream)
$xml = $reader.ReadToEnd()
$reader.Close()
$stream.Close()
$zip.Dispose()

if ($xml -match '<id>([^<]+)</id>') {
    $packageId = $matches[1]
    Write-Host "Package ID: $packageId" -ForegroundColor Cyan
}

if ($xml -match '<version>([^<]+)</version>') {
    $version = $matches[1]
    Write-Host "Package Version: $version" -ForegroundColor $(if ($version -eq "2.0.0-alpha2") { "Green" } else { "Yellow" })
} else {
    Write-Host "ERROR: Could not find version in package metadata" -ForegroundColor Red
    exit 1
}

# Push package
Write-Host "`nPushing package to NuGet.org..." -ForegroundColor Yellow
$result = dotnet nuget push $pkg.FullName --api-key $apiKey --source https://api.nuget.org/v3/index.json --skip-duplicate 2>&1

Write-Host $result

if ($LASTEXITCODE -eq 0) {
    Write-Host "`nSUCCESS: Package pushed successfully!" -ForegroundColor Green
    Write-Host "Package should be available at: https://www.nuget.org/packages/$packageId/" -ForegroundColor Cyan
} else {
    Write-Host "`nFAILED: Push failed with exit code $LASTEXITCODE" -ForegroundColor Red
    exit 1
}

