$ErrorActionPreference = 'Continue'
$output = dotnet test src/TSLPatcher.Tests/TSLPatcher.Tests.csproj --filter "FullyQualifiedName~NCSRoundtripTests" --logger "console;verbosity=normal" 2>&1
$output | Out-File -FilePath "test-results.txt" -Encoding utf8
Write-Host "Test completed. Results saved to test-results.txt"
Get-Content "test-results.txt" -Tail 100



