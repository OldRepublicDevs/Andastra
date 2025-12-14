# Test runner script
$ErrorActionPreference = 'Continue'
Write-Host "Starting test run..." -ForegroundColor Green

try {
    $result = dotnet test src/TSLPatcher.Tests/TSLPatcher.Tests.csproj `
        --filter "FullyQualifiedName~NCSRoundtripTests" `
        --logger "console;verbosity=normal" `
        --logger "trx;LogFileName=test-results.trx" `
        2>&1 | Tee-Object -FilePath "test-output.log"
    
    Write-Host "`n=== Test Output ===" -ForegroundColor Cyan
    $result | Write-Host
    
    if (Test-Path "test-results.trx") {
        Write-Host "`nTest results file created: test-results.trx" -ForegroundColor Green
    }
    
    Write-Host "`n=== Last 50 lines of output ===" -ForegroundColor Cyan
    Get-Content "test-output.log" -Tail 50 | Write-Host
} catch {
    Write-Host "Error: $_" -ForegroundColor Red
    $_.Exception | Format-List -Force | Out-String | Write-Host
}



