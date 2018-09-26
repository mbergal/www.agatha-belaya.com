$ErrorActionPreference = "Stop"

$commandLineArgs = $args

function Bootstrap()
    {
    $thisDir = Split-Path -Parent $PSCommandPath
    $nugetPath = Join-Path $thisDir "tools\nuget.exe"
    $toolsPackagesDir = (Join-Path $thisDir tools\packages)
    . $nugetPath Install (Join-Path $thisDir tools\packages.config) -o $toolsPackagesDir
    
    $psakeDir = ls "$toolsPackagesDir\psake.*" | Select-Object -First 1

    Write-Host "Args: ", $commandLineArgs

    .$psakeDir\tools\psake.ps1 $PSCommandPath $commandLineArgs -ScriptPath $psakeDir\tools
    if ($psake.build_success -eq $false) { exit 1 } else { exit 0 }
    }

if ( $psake.version -eq $null )
    {
    Bootstrap
    }


Properties `
    {
    $rootDir = (Split-Path -Parent $PSCommandPath)
    $toolsDir = (Join-Path $rootDir tools)
    $deployDir = (Join-Path $rootDir deploy)
    }

task default -depends Serve

task Build `
    {
    # msbuild15 agatha2.csproj 
    }

task Serve -depends Build `
    {
    Get-Process iisexpress -ErrorAction SilentlyContinue | Stop-Process
    . start (Join-Path ${env:ProgramFiles(x86)} "IIS Express\iisexpress") /path:$PSScriptRoot    
    Start-Sleep -s 3
    }

task Download -depends Serve `
    {
    . "$toolsDir\wget.exe" http://localhost:8080/ -m -r -k  -E -H -K -p --adjust-extension --span-hosts --convert-links --backup-converted --page-requisites --directory-prefix="$deployDir" >build.log # 2>&1 
    Get-Process iisexpress -ErrorAction SilentlyContinue | Stop-Process
    }