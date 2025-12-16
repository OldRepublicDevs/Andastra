# Fix missing using statements in test files
$testDir = Join-Path $PSScriptRoot "..\src\Andastra\Tests" | Resolve-Path

$filesToFix = @(
    "ConfigReaderGFFPathTests.cs",
    "ConfigReaderTLKTests.cs",
    "GFFFieldTypeTests.cs",
    "GFFIntegrationTests.cs",
    "GffModificationTests.cs",
    "GffModsTests.cs",
    "GFFModsUnitTests.cs",
    "IntegrationTestBase.cs",
    "LocalizedStringDeltaTests.cs",
    "NCSOptimizerTests.cs",
    "SSFIntegrationTests.cs",
    "SsfModificationTests.cs",
    "SsfModsTests.cs",
    "SSFModsUnitTests.cs",
    "TalkTableTests.cs",
    "TLKIntegrationTests.cs",
    "TlkModificationTests.cs",
    "TlkModsTests.cs",
    "TLKModsUnitTests.cs",
    "TSLPatcherTests.cs",
    "TwoDaAddColumnTests.cs",
    "TwoDaAddRowTests.cs",
    "TwoDAAdvancedTests.cs",
    "TwoDaChangeRowTests.cs",
    "TwoDACopyRowTests.cs",
    "TwoDAIntegrationTests.cs",
    "TwoDAModsAddColumnUnitTests.cs",
    "TwoDAModsAddRowAdvancedTests.cs",
    "TwoDAModsCopyRowUnitTests.cs",
    "TwoDaModsTests.cs",
    "TwoDAModsUnitTests.cs"
)

foreach ($fileName in $filesToFix) {
    $file = Get-ChildItem -Path $testDir -Filter $fileName -Recurse | Select-Object -First 1
    if ($null -eq $file) {
        Write-Host "File not found: $fileName"
        continue
    }
    
    $content = Get-Content $file.FullName -Raw
    if ($content -match 'using\s+Andastra\.Parsing\.Common\s*;') {
        Write-Host "Already has using: $fileName"
        continue
    }
    
    $lines = Get-Content $file.FullName
    $lastUsingIndex = -1
    for ($i = 0; $i -lt $lines.Count; $i++) {
        if ($lines[$i] -match '^\s*using\s+Andastra\.Parsing\s*;') {
            $lastUsingIndex = $i
        }
        elseif ($lines[$i] -match '^\s*namespace\s+' -and $lastUsingIndex -ge 0) {
            break
        }
    }
    
    if ($lastUsingIndex -ge 0) {
        $newLines = @()
        for ($i = 0; $i -le $lastUsingIndex; $i++) {
            $newLines += $lines[$i]
        }
        $newLines += "using Andastra.Parsing.Common;"
        for ($i = $lastUsingIndex + 1; $i -lt $lines.Count; $i++) {
            $newLines += $lines[$i]
        }
        
        $newContent = $newLines -join "`r`n"
        Set-Content -Path $file.FullName -Value $newContent -NoNewline
        Write-Host "Fixed: $fileName"
    }
    else {
        Write-Host "Could not find insertion point: $fileName"
    }
}

Write-Host "Done!"

