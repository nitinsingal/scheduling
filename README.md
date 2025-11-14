# SchedulingAPP in C Sharp

## Prerequisites
- .NET 8.0 SDK
- Git
- VS Code with C# Dev Kit extension (for debugging)

## Commands
### Build
```bash
dotnet build
```

### Run
```bash
dotnet run
```


## Python Integration

Python bindings are available in the `pybinding/` folder. See [pybinding/PYTHON_INTEGRATION.md](pybinding/PYTHON_INTEGRATION.md) for details.

### Quick Start (Python)
```bash
# Create virtual environment
python -m venv venv
.\venv\Scripts\Activate.ps1

# Install dependencies
pip install -r pybinding\requirements.txt

# Run example
python pybinding\example_usage.py
```

## Debugging Main Application in VS Code

### Quick Start
1. Install **C# Dev Kit** extension in VS Code
2. Restart VS Code
3. Press `F5` to start debugging
4. Choose configuration:
   - ".NET Core Launch (console)" - Debug main application


### Setting Breakpoints
- Click in gutter (left of line numbers) to add breakpoint
- Red dot = breakpoint set
- Breakpoints work in tests and model files
- Press `F9` to toggle breakpoint on current line


### Debugging a Test
Modify the TestDriver to mention a test that you want to debug and buiild using donet build command. Then
- On Power Shell/Command Prompt
- dotnet test --filter "FullyQualifiedName=TestDriver.DebugTest" --configuration Debug --no-build --verbosity normal
- Use Net Core Attach from launch.json and pick the process from Terminal window to attach and debug any test mentioned in TestDriver class

### Run Scheduling Tests
- dotnet test Scheduling.Tests/Scheduling.Tests.csproj --filter "FullyQualifiedName~Scheduling.Tests" --verbosity normal -- --parallel none

### Run a selected Group of Tests
- dotnet test Scheduling.Tests/Scheduling.Tests.csproj --filter "FullyQualifiedName~Scheduling.Tests.ProductTests" --verbosity normal -- --parallel none
