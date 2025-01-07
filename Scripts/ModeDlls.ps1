# Set the paths to the source and destination folders
$debug = "bin/Debug/net8.0"
$wwwrootModules = "../ModularShell/wwwroot/Modules"

# Create the destination root folder if it doesn't exist
New-Item -ItemType Directory -Path $wwwrootModules -Force

# Define the list of DLLs and their corresponding source project folders
$dllsToCopy = @(
    @{ Name = "DummyDependency"; ProjectFolder = "../DummyDependency" }
    @{ Name = "RazorCLDependent"; ProjectFolder = "../RazorCLDependent" }
    @{ Name = "RazorCLSharedDependent"; ProjectFolder = "../RazorCLSharedDependent" }
    @{ Name = "RazorCLStandalone"; ProjectFolder = "../RazorCLStandalone" }
)

# Loop through the list of DLLs
foreach ($dllInfo in $dllsToCopy) {
    # Construct the source DLL path
    $dllPath = Join-Path -Path $dllInfo.ProjectFolder -ChildPath "$debug/$($dllInfo.Name).dll"

    # Construct the destination path
    $destinationPath = Join-Path -Path $wwwrootModules -ChildPath $dllInfo.Name

    # Create the destination directory if it doesn't exist
    New-Item -ItemType Directory -Force -Path $destinationPath

    # Copy the DLL
    Write-Host "Copying $($dllInfo.Name).dll from $($dllPath) to $($destinationPath)"
    Copy-Item -Path $dllPath -Destination $destinationPath -Force
}

Write-Host "DLLs copied successfully!"