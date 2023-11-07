@echo off
setlocal

:: Wait for 15 seconds
::timeout /t 15

:: Set these variables
set "REPO_OWNER=Kubson3000"
set "REPO_NAME=TimeCalculator"

:: Get the URL of the latest release zip file
for /f "delims=" %%i in ('curl -s https://api.github.com/repos/%REPO_OWNER%/%REPO_NAME%/releases/latest ^| findstr browser_download_url') do set "LATEST_ZIP_URL=%%i"
set "LATEST_ZIP_URL=%LATEST_ZIP_URL:*browser_download_url": =%"
set "LATEST_ZIP_URL=%LATEST_ZIP_URL:~0,-1%"

:: Download the zip file
curl -L -o latest.zip "%LATEST_ZIP_URL%"

:: Extract the zip file using PowerShell
powershell -command "$dest_dir = 'C:\Program Files (x86)\Time_Calculator\'; Expand-Archive -Path .\latest.zip -DestinationPath $dest_dir -Force"

:: Delete the zip file
del latest.zip

endlocal
