<#
.SYNOPSIS
Utility to rename engine folders and namespaces (e.g., OdysseyRuntime -> BioWareEngines) with dry-run support.

.DESCRIPTION
Renames a root folder and performs text replacements for namespaces/usings/project references across solution files.
Designed to be flexible for future engine renames by parameterizing names, file globs, and exclusions.

.EXAMPLE
.\EngineNamespaceRenamer.ps1 -RootPath "g:\GitHub\HoloPatcher.NET" -OldFolderName "OdysseyRuntime" -NewFolderName "BioWareEngines" -OldNamespace "Odyssey" -NewNamespace "BioWareEngines" -WhatIf

.EXAMPLE
.\EngineNamespaceRenamer.ps1 -OldFolderName "OldEngine" -NewFolderName "NewEngine" -OldNamespace "OldEngine" -NewNamespace "NewEngine" -IncludeFiles "*.cs","*.csproj","*.sln" -Verbose
#>
[CmdletBinding(SupportsShouldProcess = $true, ConfirmImpact = 'High')]
param(
    [Parameter(Mandatory = $false)]
    [string]$RootPath = (Get-Location).Path,

    [Parameter(Mandatory = $true)]
    [string]$OldFolderName,

    [Parameter(Mandatory = $true)]
    [string]$NewFolderName,

    [Parameter(Mandatory = $true)]
    [string]$OldNamespace,

    [Parameter(Mandatory = $true)]
    [string]$NewNamespace,

    [Parameter(Mandatory = $false)]
    [string[]]$IncludeFiles = @("*.cs", "*.csproj", "*.sln"),

    [Parameter(Mandatory = $false)]
    [string[]]$ExcludeDirectories = @("bin", "obj", ".git", ".vs", "packages", "node_modules", "dist", "vendor"),

    [Parameter(Mandatory = $false)]
    [switch]$NoFolderRename,

    [Parameter(Mandatory = $false)]
    [switch]$NoTextReplace
)

function Test-IsExcludedDirectory {
    param(
        [string]$Path,
        [string[]]$ExcludedNames
    )

    foreach ($name in $ExcludedNames) {
        if ($Path -like "*\${name}\*" -or $Path -like "*\${name}") {
            return $true
        }
    }

    return $false
}

function Invoke-ReplaceInFile {
    param(
        [string]$FilePath,
        [string]$OldValue,
        [string]$NewValue
    )

    $original = Get-Content -Path $FilePath -Raw
    $updated = $original.Replace($OldValue, $NewValue)

    if ($updated -ne $original) {
        if ($PSCmdlet.ShouldProcess($FilePath, "Replace '$OldValue' with '$NewValue'")) {
            Set-Content -Path $FilePath -Value $updated
        }
        return $true
    }

    return $false
}

Write-Verbose "RootPath: $RootPath"
Write-Verbose "OldFolderName: $OldFolderName"
Write-Verbose "NewFolderName: $NewFolderName"
Write-Verbose "OldNamespace: $OldNamespace"
Write-Verbose "NewNamespace: $NewNamespace"
Write-Verbose "Includes: $($IncludeFiles -join ', ')"
Write-Verbose "Excludes: $($ExcludeDirectories -join ', ')"

$resolvedRoot = Resolve-Path -Path $RootPath

if (-not $NoFolderRename) {
    $oldFolder = Join-Path $resolvedRoot $OldFolderName
    $newFolder = Join-Path $resolvedRoot $NewFolderName

    if (Test-Path -Path $oldFolder) {
        if (Test-Path -Path $newFolder) {
            throw "Target folder already exists: $newFolder"
        }

        if ($PSCmdlet.ShouldProcess($oldFolder, "Rename to $newFolder")) {
            Move-Item -Path $oldFolder -Destination $newFolder
        }
    }
    else {
        Write-Warning "Old folder not found: $oldFolder"
    }
}

if (-not $NoTextReplace) {
    $files = Get-ChildItem -Path $resolvedRoot -Recurse -File -Include $IncludeFiles |
        Where-Object { -not (Test-IsExcludedDirectory -Path $_.FullName -ExcludedNames $ExcludeDirectories) }

    $changedCount = 0
    foreach ($file in $files) {
        $didChange = Invoke-ReplaceInFile -FilePath $file.FullName -OldValue $OldNamespace -NewValue $NewNamespace
        if ($didChange) {
            $changedCount++
            Write-Verbose "Updated: $($file.FullName)"
        }
    }

    Write-Host "Files updated: $changedCount"
}

Write-Host "Completed. Use -WhatIf for dry-run and -Verbose for detailed logging."

