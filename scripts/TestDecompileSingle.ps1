# Simple test script to decompile a single NCS file and show the output
param(
    [string]$NcsFile = "roundtrip-work/k1/K1/Data/scripts.bif/k_act_canderadd.ncs",
    [string]$OutputFile = "test-decompiled.nss"
)

$ErrorActionPreference = "Stop"

Write-Host "Decompiling: $NcsFile"
Write-Host "Output: $OutputFile"
Write-Host ""

# Build the project first
Write-Host "Building CSharpKOTOR..."
dotnet build src/CSharpKOTOR/CSharpKOTOR.csproj --no-incremental | Out-Null
if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!"
    exit 1
}

# Use the FileDecompiler to decompile
$decompilerCode = @"
using System;
using System.IO;
using CSharpKOTOR.Formats.NCS.NCSDecomp;
using CSharpKOTOR.Common;

class TestDecompile {
    static void Main(string[] args) {
        string ncsFile = args[0];
        string outputFile = args[1];
        
        try {
            FileDecompiler decompiler = new FileDecompiler(ncsFile, Game.K1);
            string decompiled = decompiler.Decompile();
            File.WriteAllText(outputFile, decompiled);
            Console.WriteLine("Decompilation successful!");
            Console.WriteLine("Output:");
            Console.WriteLine("====");
            Console.WriteLine(decompiled);
            Console.WriteLine("====");
        } catch (Exception e) {
            Console.Error.WriteLine("Error: " + e.Message);
            Console.Error.WriteLine(e.StackTrace);
            Environment.Exit(1);
        }
    }
}
"@

$tempCsFile = "temp-test-decompile.cs"
$decompilerCode | Out-File -FilePath $tempCsFile -Encoding UTF8

try {
    Write-Host "Compiling test program..."
    dotnet run --project src/CSharpKOTOR/CSharpKOTOR.csproj -- "$NcsFile" "$OutputFile" 2>&1 | Write-Host
    
    if (Test-Path $OutputFile) {
        Write-Host ""
        Write-Host "=== DECOMPILED OUTPUT ==="
        Get-Content $OutputFile
    } else {
        Write-Host "Output file not created!"
    }
} finally {
    if (Test-Path $tempCsFile) {
        Remove-Item $tempCsFile -ErrorAction SilentlyContinue
    }
}

