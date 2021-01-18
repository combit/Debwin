#############################################################################################################
#	This script merges the files of the last release build to a single assembly                             #
#############################################################################################################

function MergeAssemblies
{
    param([string] $outputFile, [string] $mainAssembly, [string[]] $refDlls)
    $ilmergePath = $PSScriptRoot + "\packages\ILMerge.3.0.41\tools\net452\ILMerge.exe"


    if (!(Test-Path($ilmergePath))) {
        Write-Error "ILMerge was not found ($ilmergePath)"
        return
    }

    $mergeArgs = @("/out:" + $OutputFile)
    $mergeArgs += $mainAssembly
    $mergeArgs += $refDlls

    Write-Host "Merging Assemblies: "
	Write-Host $ilmergePath $mergeArgs
    & $ilmergePath $mergeArgs
    Write-Host "Finished Merging Assemblies"
}

Clear-Host

$buildConfiguration = "Release"
Write-Host "Build Configuration:"
Write-Host $buildConfiguration  -ForegroundColor Yellow

# Goto Output directory
Push-Location ($PSScriptRoot + "\Debwin.UI\bin\" + $buildConfiguration)

# Merge Assemblies to one executable
$referencedDlls = [String[]](Get-ChildItem "*.dll" | Select-Object -ExpandProperty Name)   # Get file info of dlls, select names
MergeAssemblies "Debwin4.exe" "Debwin.UI.exe" $referencedDlls

Pop-Location
Write-Host "Debwin4 merge is complete!" -ForegroundColor Green
