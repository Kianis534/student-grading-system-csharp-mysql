<#
.SYNOPSIS
    Builds a ready-to-run release zip of Student Grading System.

.DESCRIPTION
    Publishes the WinForms app in Release mode and bundles it with the SQL scripts,
    the database setup script, README and LICENSE:

        artifacts/StudentGradingSystem-<version>-windows.zip

    The zip always contains the placeholder Database.config template - never your local credentials.

.EXAMPLE
    .\scripts\package-release.ps1
#>
[CmdletBinding()]
param(
    [string] $Configuration = 'Release',
    [string] $OutputDirectory
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $PSScriptRoot
if (-not $OutputDirectory) { $OutputDirectory = Join-Path $repoRoot 'artifacts' }

[xml] $props = Get-Content (Join-Path $repoRoot 'Directory.Build.props')
$version = ($props.Project.PropertyGroup | Where-Object { $_.Version } | Select-Object -First 1).Version
if (-not $version) { throw 'Version not found in Directory.Build.props.' }

$packageName = "StudentGradingSystem-$version-windows"
$staging = Join-Path $OutputDirectory $packageName
$zipPath = Join-Path $OutputDirectory "$packageName.zip"

if (Test-Path $staging) { Remove-Item $staging -Recurse -Force }
if (Test-Path $zipPath) { Remove-Item $zipPath -Force }
New-Item -ItemType Directory -Path $staging | Out-Null

Write-Host "Publishing $packageName ..."
& dotnet publish (Join-Path $repoRoot 'src\StudentGradingSystem\StudentGradingSystem.csproj') `
    --configuration $Configuration --output $staging --nologo
if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed with exit code $LASTEXITCODE." }

# Never ship whatever local credentials the publish step copied: always use the template.
Copy-Item (Join-Path $repoRoot 'src\StudentGradingSystem\Database.example.config') (Join-Path $staging 'Database.config') -Force
Get-ChildItem $staging -Filter '*.xml' | Remove-Item -Force

$sqlTarget = Join-Path $staging 'database'
New-Item -ItemType Directory -Path $sqlTarget | Out-Null
Copy-Item (Join-Path $repoRoot 'database\*.sql') $sqlTarget
Copy-Item (Join-Path $repoRoot 'scripts\setup-database.ps1') $staging
Copy-Item (Join-Path $repoRoot 'README.md') $staging
Copy-Item (Join-Path $repoRoot 'LICENSE') (Join-Path $staging 'LICENSE.txt')

if (Select-String -Path (Join-Path $staging 'Database.config') -Pattern 'YOUR_MYSQL_PASSWORD' -Quiet) {
    Write-Host 'Verified: package contains only the placeholder Database.config.'
} else {
    throw 'Refusing to package: Database.config in the staging folder is not the placeholder template.'
}

Compress-Archive -Path (Join-Path $staging '*') -DestinationPath $zipPath -CompressionLevel Optimal
Write-Host "Created $zipPath" -ForegroundColor Green
