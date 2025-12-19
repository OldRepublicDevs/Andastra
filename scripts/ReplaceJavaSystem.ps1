# Script to replace JavaSystem.@out.Println and JavaSystem.@err.Println with DecompilerLogger
# Compatible with C# 7.3 and .NET 4.6.2

$files = @(
    "src\Andastra\Parsing\Resource\Formats\NCS\NCSDecomp\CompilerExecutionWrapper.cs",
    "src\Andastra\Parsing\Resource\Formats\NCS\NCSDecomp\CompilerUtil.cs",
    "src\Andastra\Parsing\Resource\Formats\NCS\NCSDecomp\Decoder.cs",
    "src\Andastra\Parsing\Resource\Formats\NCS\NCSDecomp\DoGlobalVars.cs",
    "src\Andastra\Parsing\Resource\Formats\NCS\NCSDecomp\DoTypes.cs",
    "src\Andastra\Parsing\Resource\Formats\NCS\NCSDecomp\FileDecompiler.cs",
    "src\Andastra\Parsing\Resource\Formats\NCS\NCSDecomp\MainPass.cs",
    "src\Andastra\Parsing\Resource\Formats\NCS\NCSDecomp\NameGenerator.cs",
    "src\Andastra\Parsing\Resource\Formats\NCS\NCSDecomp\NodeUtils.cs",
    "src\Andastra\Parsing\Resource\Formats\NCS\NCSDecomp\NoOpRegistrySpoofer.cs",
    "src\Andastra\Parsing\Resource\Formats\NCS\NCSDecomp\PcodeReaderTest.cs",
    "src\Andastra\Parsing\Resource\Formats\NCS\NCSDecomp\RegistrySpoofer.cs",
    "src\Andastra\Parsing\Resource\Formats\NCS\NCSDecomp\SetPositions.cs",
    "src\Andastra\Parsing\Resource\Formats\NCS\NCSDecomp\StructType.cs",
    "src\Andastra\Parsing\Resource\Formats\NCS\NCSDecomp\SubroutineState.cs",
    "src\Andastra\Parsing\Resource\Formats\NCS\NCSDecomp\SubScriptState.cs",
    "src\Andastra\Parsing\Resource\Formats\NCS\NCSDecomp\Analysis\PrototypeEngine.cs",
    "src\Andastra\Parsing\Resource\Formats\NCS\NCSDecomp\Node\ARsaddCmd.cs",
    "src\Andastra\Parsing\Resource\Formats\NCS\NCSDecomp\Node\ARsaddCommand.cs",
    "src\Andastra\Parsing\Resource\Formats\NCS\NCSDecomp\ScriptNode\ASub.cs",
    "src\Andastra\Parsing\Resource\Formats\NCS\NCSDecomp\Utils\FileScriptData.cs",
    "src\Andastra\Parsing\Resource\Formats\NCS\NCSDecomp\Utils\SubroutineAnalysisData.cs",
    "src\Andastra\Parsing\Resource\Formats\NCS\NCSBinaryReader.cs"
)

# Get the repository root (parent of scripts directory)
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$rootDir = Split-Path -Parent $scriptDir

foreach ($file in $files) {
    $fullPath = Join-Path $rootDir $file
    if (-not (Test-Path $fullPath)) {
        Write-Warning "File not found: $fullPath"
        continue
    }

    Write-Host "Processing: $file"
    $content = Get-Content $fullPath -Raw -Encoding UTF8

    # Check if file already has the using statement
    $hasUsingStatic = $content -match "using static.*DecompilerLogger"
    $hasUsingJavaSystem = $content -match "using.*JavaSystem"

    # Add using static if not present (after the last using statement)
    if (-not $hasUsingStatic) {
        # Find the last using statement
        if ($content -match "(?s)(.*?)(using[^;]+;)(.*)") {
            $before = $matches[1]
            $lastUsing = $matches[2]
            $after = $matches[3]
            
            # Add the static using after the last using
            $newUsing = "using static Andastra.Parsing.Formats.NCS.NCSDecomp.DecompilerLogger;"
            $content = $before + $lastUsing + "`r`n" + $newUsing + $after
        }
    }

    # Remove JavaSystem using alias if present
    $content = $content -replace "using\s+JavaSystem\s*=\s*[^;]+;", ""

    # Replace JavaSystem.@out.Println with Debug
    $content = $content -replace "JavaSystem\.@out\.Println\s*\(", "Debug("
    
    # Replace JavaSystem.@err.Println with Error
    $content = $content -replace "JavaSystem\.@err\.Println\s*\(", "Error("

    # Write back
    Set-Content -Path $fullPath -Value $content -Encoding UTF8 -NoNewline
    Write-Host "  Updated: $file"
}

Write-Host "`nDone! All files processed."

