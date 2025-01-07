# Set the paths to the source and destination folders
$debug = "bin/Debug/net8.0"
$wwwrootModules = "../ModularShell/wwwroot/Modules"

# Create the destination folders if they don't exist
New-Item -ItemType Directory -Path $wwwrootModules -Force

# Move the DummyDependency.dll
Move-Item -Path "../DummyDependency/$debug/DummyDependency.dll" -Destination "$wwwrootModules/DummyDependency" -Force

# Move the RazorCLDependent.dll
Move-Item -Path "../RazorCLDependent/$debug/RazorCLDependent.dll" -Destination "$wwwrootModules/RazorCLDependent" -Force

# Move the RazorCLStandalone.dll
Move-Item -Path "../RazorCLStandalone/$debug/RazorCLStandalone.dll" -Destination "$wwwrootModules/RazorCLStandalone" -Force