# Define paths
$RootPath = $PSScriptRoot
$UpdateTempPath = "$RootPath\update_temp"

# Define files and folders to keep
$KeepFolders = @("Profiles", "TestCases", "update_temp")
$KeepFiles = @("update.ps1")

# Step 1: Delete everything EXCEPT the allowed folders and update.ps1
Write-Host "Cleaning up old files..."
Get-ChildItem -Path $RootPath -Force | Where-Object {
    ($_.PSIsContainer -and $_.Name -notin $KeepFolders) -or
    ($_.Name -notin $KeepFiles -and -not $_.PSIsContainer)
} | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue

# Step 2: Check if update_temp exists before proceeding
if (Test-Path $UpdateTempPath) {
    Write-Host "Updating files..."
    Get-ChildItem -Path $UpdateTempPath -Force | Move-Item -Destination $RootPath -Force
} else {
    Write-Host "WARNING: update_temp folder not found! Skipping update."
}

# Step 3: Remove update_temp folder if it exists
if (Test-Path $UpdateTempPath) {
    Write-Host "Removing update_temp..."
    Remove-Item -Path $UpdateTempPath -Recurse -Force -ErrorAction SilentlyContinue
}

# Step 4: Start the application (only if it exists)
$AppPath = "$RootPath\QaseTestCaseGenerator.exe"
if (Test-Path $AppPath) {
    Write-Host "Starting QaseTestCaseGenerator.exe..."
    Start-Process -FilePath $AppPath
} else {
    Write-Host "ERROR: QaseTestCaseGenerator.exe not found! Update may have failed."
}

Write-Host "Update completed!"
