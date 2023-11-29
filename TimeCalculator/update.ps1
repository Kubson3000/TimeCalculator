Get-Process "TimeCalculator" -ErrorAction Ignore | Stop-Process

# Define the user and repo
$userRepo = "Kubson3000/TimeCalculator"

# Define the destination path
$destinationPath = Split-Path -Parent $MyInvocation.MyCommand.Definition

# Define the URL for the latest release
$url = "https://api.github.com/repos/$userRepo/releases/latest"

# Download the latest release info
$release = Invoke-RestMethod -Uri $url

# Get the URL of the zip file
$zipUrl = $release.assets | Where-Object { $_.name -like "*.zip" } | Select-Object -First 1 -ExpandProperty browser_download_url

# Define the path for the downloaded zip file
$zipFile = Join-Path $env:TEMP ([IO.Path]::GetFileName($zipUrl))

# Download the zip file
Invoke-WebRequest -Uri $zipUrl -OutFile $zipFile

# Unzip the file to the destination path
Expand-Archive -Path $zipFile -DestinationPath $destinationPath -Force

# Delete the downloaded zip file
Remove-Item -Path $zipFile
