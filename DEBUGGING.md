# Debugging Guide for Scheduling Project

## Quick Start

1. **Install C# Dev Kit Extension:**
   - Open VS Code
   - Go to Extensions (Ctrl+Shift+X)
   - Search for "C# Dev Kit"
   - Click Install
   - Restart VS Code

2. **Start Debugging:**
   - Open any C# file
   - Press `F5` to start debugging
   - Choose the configuration you want

## Debug Configurations

### 1. Debug Main Application
**When to use:** Debug the main console application

**How:**
- Press `F5`
- Select ".NET Core Launch (console)"
- Breakpoints in `Program.cs` or model files will be hit

### 2. Debug All Tests
**When to use:** Run all tests with debugger attached

**How:**
- Press `F5`
- Select ".NET Core Test Explorer"
- All tests will run
- Debugger stops at any breakpoints in tests or models

### 3. Debug Current Test File
**When to use:** Debug only tests in the current file

**How:**
- Open a test file (e.g., `ProductLocationTests.cs`)
- Press `F5`
- Select ".NET Core Test (Current File)"
- Only tests in that file will run

### 4. Debug Specific Test Method
**When to use:** Focus on one failing test

**How:**
- Set a breakpoint in the test method
- Use Debug Current Test File configuration
- Or right-click test method → "Debug Test"

## Setting Breakpoints

### Basic Breakpoints
- Click in the gutter (left margin) next to line numbers
- Red dot appears = breakpoint set
- Click again to remove

### Conditional Breakpoints
- Right-click on breakpoint → Edit Breakpoint
- Add condition (e.g., `quantity > 100`)
- Debugger only stops when condition is true

### Logpoints
- Right-click in gutter → Add Logpoint
- Enter message like `Quantity = {quantity}`
- Prints to Debug Console without stopping

## Debugging Features

### Variables Panel
- Shows all local variables
- Expands objects to see properties
- Updates as you step through code

### Watch Panel
- Add expressions to monitor
- Right-click variable → Add to Watch
- Or manually type expression

### Call Stack
- Shows the path that led to current line
- Click to jump to different stack frame
- Helps understand how you got there

### Debug Console
- Evaluate expressions while paused
- Type variable names to see values
- Execute code: `productLocation.GetCumulativeInventory(time1)`

## Common Debugging Scenarios

### Debug a Failing Test
```
1. Open test file (e.g., ProductLocationTests.cs)
2. Find failing test method
3. Set breakpoint at start of test
4. Press F5 → ".NET Core Test (Current File)"
5. Step through with F10/F11
6. Inspect variables in Variables panel
```

### Debug Inventory Calculations
```
1. Open ProductLocation.cs
2. Set breakpoint in AddInventory method
3. Run tests with F5
4. When breakpoint hits, check:
   - _inventoryProfile state
   - time parameter value
   - quantity parameter value
5. Step into InventoryProfile.AddInventory with F11
```

### Debug Test Setup Issues
```
1. Set breakpoint in test Arrange section
2. Run test
3. Check that:
   - ProductLocation created correctly
   - Product and Location exist
   - InventoryProfile initialized
```

## Keyboard Shortcuts

| Action | Shortcut |
|--------|----------|
| Start/Continue | F5 |
| Step Over | F10 |
| Step Into | F11 |
| Step Out | Shift+F11 |
| Stop Debugging | Shift+F5 |
| Toggle Breakpoint | F9 |
| Restart | Ctrl+Shift+F5 |

## Tips

1. **Step Into vs Step Over:**
   - Step Into (F11): Goes into method calls
   - Step Over (F10): Executes method and stops at next line

2. **Inspect Collections:**
   - Click arrow next to List/Dictionary in Variables panel
   - See all items and their values

3. **Evaluate Complex Expressions:**
   - Use Debug Console
   - Type `productLocation.GetAllInventoryChanges()` while paused
   - See results immediately

4. **Multiple Breakpoints:**
   - Set breakpoints in both test and model files
   - Trace execution flow from test into model

5. **Hover for Quick Info:**
   - Hover over any variable while paused
   - See current value without opening Variables panel

## Troubleshooting

### Debugger Not Stopping at Breakpoints
- Ensure code is built (Ctrl+Shift+B)
- Check breakpoint is red (not hollow)
- Verify you're running debug configuration (not release)

### Can't See Variable Values
- Make sure you're paused at a breakpoint
- Check variable is in scope
- Use Debug Console to evaluate manually

### Tests Not Found
- Build test project: `dotnet build Scheduling.Tests/Scheduling.Tests.csproj`
- Check test methods have `[Fact]` attribute
- Ensure using Xunit namespace

### Extension Issues
- Reload VS Code window (Ctrl+Shift+P → "Reload Window")
- Reinstall C# Dev Kit extension
- Check .NET SDK installed: `dotnet --version`

