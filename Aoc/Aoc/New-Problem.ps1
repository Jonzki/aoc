[CmdletBinding()]
param()

function Read-IntegerInRange {
    param(
        [Parameter(Mandatory)]
        [string] $Prompt,

        [Parameter(Mandatory)]
        [int] $Minimum,

        [Parameter(Mandatory)]
        [int] $Maximum
    )

    while ($true) {
        $value = Read-Host $Prompt
        $number = 0

        if ([int]::TryParse($value, [ref] $number) -and $number -ge $Minimum -and $number -le $Maximum) {
            return $number
        }

        Write-Host "Enter a number from $Minimum to $Maximum."
    }
}

$year = Read-IntegerInRange -Prompt 'Advent of Code year' -Minimum 2015 -Maximum 2099
$day = Read-IntegerInRange -Prompt 'Problem number (1-25)' -Minimum 1 -Maximum 25

$yearSuffix = ($year % 100).ToString('D2')
$dayText = $day.ToString('D2')
$yearDirectory = "Aoc$yearSuffix"
$solutionRoot = Split-Path -Parent $PSScriptRoot

$inputPath = Join-Path $solutionRoot "Aoc/Inputs/$year/$year.$dayText.txt"
$problemPath = Join-Path $solutionRoot "Aoc/Problems/$yearDirectory/Problem$dayText.cs"
$testsPath = Join-Path $solutionRoot "Aoc.Tests/Problems/$yearDirectory/Problem${dayText}Tests.cs"
$problemTemplate = Join-Path $solutionRoot 'Aoc/Problems/Aoc00/Problem99.cs.template'
$testsTemplate = Join-Path $solutionRoot 'Aoc.Tests/Problems/Aoc00/Problem99Tests.cs.template'

foreach ($templatePath in @($problemTemplate, $testsTemplate)) {
    if (-not (Test-Path -LiteralPath $templatePath -PathType Leaf)) {
        throw "Template file not found: $templatePath"
    }
}

$existingCodePaths = @($problemPath, $testsPath) | Where-Object { Test-Path -LiteralPath $_ }
if ($existingCodePaths.Count -gt 0) {
    $existingCodePaths | ForEach-Object { Write-Error "Refusing to overwrite existing file: $_" }
    exit 1
}

if (Test-Path -LiteralPath $inputPath -PathType Container) {
    throw "Expected an input file, but found a directory at: $inputPath"
}

$inputAlreadyExists = Test-Path -LiteralPath $inputPath -PathType Leaf
$createdFiles = [System.Collections.Generic.List[string]]::new()

try {
    foreach ($directory in @((Split-Path -Parent $inputPath), (Split-Path -Parent $problemPath), (Split-Path -Parent $testsPath))) {
        [System.IO.Directory]::CreateDirectory($directory) | Out-Null
    }

    if (-not $inputAlreadyExists) {
        [System.IO.File]::WriteAllText($inputPath, '')
        $createdFiles.Add($inputPath)
    }

    foreach ($copy in @(
        @{ Template = $problemTemplate; Destination = $problemPath },
        @{ Template = $testsTemplate; Destination = $testsPath }
    )) {
        $createdFiles.Add($copy.Destination)
        Copy-Item -LiteralPath $copy.Template -Destination $copy.Destination

        $content = [System.IO.File]::ReadAllText($copy.Destination)
        $content = $content.Replace('99', $dayText).Replace('00', $yearSuffix)
        [System.IO.File]::WriteAllText($copy.Destination, $content)
    }
}
catch {
    foreach ($path in $createdFiles) {
        Remove-Item -LiteralPath $path -Force -ErrorAction SilentlyContinue
    }

    throw
}

Write-Host "Created problem scaffold for $year day ${dayText}:"
if ($inputAlreadyExists) {
    Write-Host "  Kept existing input: $inputPath"
}
else {
    Write-Host "  Created input: $inputPath"
}
Write-Host "  $problemPath"
Write-Host "  $testsPath"
