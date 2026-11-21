<#
.SYNOPSIS
    Creates the StudentGradingSystem MySQL database and writes the app's Database.config.

.DESCRIPTION
    Runs database/01-schema.sql and (unless -SkipSeed) database/02-seed.sql with the MySQL
    command-line client, then saves a matching connection string to Database.config so the
    app can sign in straight away.

    Works both from a clone of the repository and from the extracted release zip.
    WARNING: re-running it drops and recreates the StudentGradingSystem tables.

.EXAMPLE
    .\scripts\setup-database.ps1
    Prompts for the MySQL root password and sets everything up on localhost:3306.

.EXAMPLE
    .\scripts\setup-database.ps1 -Server db.example.local -Port 3307 -User sgs_admin -SkipSeed
#>
[CmdletBinding()]
param(
    [string] $Server = 'localhost',
    [ValidateRange(1, 65535)]
    [int] $Port = 3306,
    [string] $User = 'root',
    [SecureString] $Password,
    [switch] $SkipSeed,
    [switch] $SkipConfig,
    [string] $MySqlPath
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Find-MySqlClient {
    param([string] $Explicit)

    if ($Explicit) {
        if (-not (Test-Path $Explicit)) { throw "mysql client not found at '$Explicit'." }
        return (Resolve-Path $Explicit).Path
    }

    $onPath = Get-Command mysql -ErrorAction SilentlyContinue
    if ($onPath) { return $onPath.Source }

    $installed = Get-ChildItem -Path "$env:ProgramFiles\MySQL\MySQL Server *\bin\mysql.exe" -ErrorAction SilentlyContinue |
        Sort-Object FullName -Descending |
        Select-Object -First 1
    if ($installed) { return $installed.FullName }

    throw "Couldn't find the MySQL command-line client (mysql.exe). Install MySQL Server, add its bin folder to PATH, or pass -MySqlPath."
}

# Release zip: script sits next to StudentGradingSystem.exe. Repository: script lives in .\scripts.
if (Test-Path (Join-Path $PSScriptRoot 'StudentGradingSystem.exe')) {
    $sqlDir     = Join-Path $PSScriptRoot 'database'
    $configPath = Join-Path $PSScriptRoot 'Database.config'
} else {
    $repoRoot   = Split-Path -Parent $PSScriptRoot
    $sqlDir     = Join-Path $repoRoot 'database'
    $configPath = Join-Path $repoRoot 'src\StudentGradingSystem\Database.config'
}

$scripts = @(Join-Path $sqlDir '01-schema.sql')
if (-not $SkipSeed) { $scripts += Join-Path $sqlDir '02-seed.sql' }
foreach ($file in $scripts) {
    if (-not (Test-Path $file)) { throw "SQL script not found: $file" }
}

$mysql = Find-MySqlClient -Explicit $MySqlPath
Write-Host "Using MySQL client: $mysql"

if (-not $Password) {
    $Password = Read-Host "MySQL password for '$User'@'$Server'" -AsSecureString
}
$bstr = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($Password)
try {
    $plainPassword = [Runtime.InteropServices.Marshal]::PtrToStringBSTR($bstr)
} finally {
    [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($bstr)
}

# The password goes into a short-lived option file instead of the command line,
# so it never shows up in the process list or shell history.
$optionFile = [IO.Path]::GetTempFileName()
try {
    $escaped = $plainPassword.Replace('\', '\\').Replace('"', '\"')
    Set-Content -Path $optionFile -Encoding ASCII -Value @('[client]', "password=""$escaped""")

    foreach ($file in $scripts) {
        Write-Host "Running $(Split-Path -Leaf $file) ..."
        $arguments = @(
            "--defaults-extra-file=`"$optionFile`"",
            "--host=$Server",
            "--port=$Port",
            "--user=$User",
            '--default-character-set=utf8mb4'
        )
        $process = Start-Process -FilePath $mysql -ArgumentList $arguments -RedirectStandardInput $file `
            -NoNewWindow -Wait -PassThru
        if ($process.ExitCode -ne 0) {
            throw "mysql exited with code $($process.ExitCode) while running $(Split-Path -Leaf $file)."
        }
    }
} finally {
    Remove-Item -Path $optionFile -Force -ErrorAction SilentlyContinue
}

Write-Host 'Database created.' -ForegroundColor Green

if (-not $SkipConfig) {
    $builder = New-Object System.Data.Common.DbConnectionStringBuilder
    $builder['Server'] = $Server
    $builder['Port'] = $Port
    $builder['Database'] = 'StudentGradingSystem'
    $builder['User ID'] = $User
    $builder['Password'] = $plainPassword
    $builder['SslMode'] = 'Preferred'
    $builder['AllowPublicKeyRetrieval'] = 'True'

    $xml = New-Object System.Xml.XmlDocument
    $xml.AppendChild($xml.CreateXmlDeclaration('1.0', 'utf-8', $null)) | Out-Null
    $xml.AppendChild($xml.CreateComment(' Student Grading System - local database settings. Git-ignored: never commit this file. ')) | Out-Null
    $root = $xml.CreateElement('appSettings')
    $add = $xml.CreateElement('add')
    $add.SetAttribute('key', 'SGS.ConnectionString')
    $add.SetAttribute('value', $builder.ConnectionString)
    $root.AppendChild($add) | Out-Null
    $xml.AppendChild($root) | Out-Null
    $xml.Save($configPath)

    Write-Host "Saved connection settings to $configPath" -ForegroundColor Green
}

$plainPassword = $null

if (-not $SkipSeed) {
    Write-Host ''
    Write-Host 'Demo accounts (change them before real use):'
    Write-Host '  instructor / Instructor@123'
    Write-Host '  jsmith     / Student@123   (also ejohnson, mbrown, sdavis, ...)'
}
