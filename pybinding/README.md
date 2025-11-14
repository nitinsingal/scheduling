# Python Bindings for Scheduling Library

This folder contains Python bindings for the Scheduling C# library using pythonnet.

## Files

- **`scheduling_py.py`** - Python wrapper module providing pythonic interface to C# classes
- **`example_usage.py`** - Comprehensive example demonstrating all features
- **`test_python_integration.py`** - Integration tests for Python bindings
- **`requirements.txt`** - Python dependencies
- **`PYTHON_INTEGRATION.md`** - Detailed documentation and API reference

## Quick Start

### 1. Prerequisites
- Python 3.12+ (recommended) or Python 3.7+
- .NET 8.0 SDK
- Built C# library (run `dotnet build` from project root)

### 2. Setup

```bash
# Create virtual environment (from project root)
python -m venv venv

# Activate virtual environment
.\venv\Scripts\Activate.ps1  # Windows PowerShell
# or
source venv/bin/activate      # Linux/Mac

# Install dependencies
pip install -r pybinding\requirements.txt
```

### 3. Run Example

```bash
python pybinding\example_usage.py
```

## Usage Example

```python
import sys
sys.path.insert(0, 'pybinding')
import scheduling_py as sched
from datetime import datetime

# Create products and locations
product = sched.Product.create("Widget")
location = sched.Location.create("Warehouse-A")

# Create product-location relationship
pl = sched.ProductLocation.create("Widget", "Warehouse-A")

# Add inventory
time = datetime(2024, 1, 1, 8, 0, 0)
sched.add_inventory(pl, time, 100.0)

# Query inventory
cumulative = sched.get_cumulative_inventory(pl, time)
print(f"Inventory: {cumulative}")
```

## API Overview

### Core Classes
- **Product** - Manage products
- **Location** - Manage locations  
- **ProductLocation** - Manage product-location relationships and inventory

### Helper Functions
- `add_inventory(pl, time, quantity)` - Add inventory
- `remove_inventory(pl, time, quantity)` - Remove inventory
- `update_inventory(pl, time, net_change)` - Update inventory change
- `get_cumulative_inventory(pl, time)` - Get cumulative inventory at time
- `get_all_inventory_changes(pl)` - Get all inventory changes
- `get_inventory_changes_in_range(pl, start, end)` - Get changes in time range

## Python 3.12+ Support

The bindings fully support Python 3.12 and later versions. The required dependencies are:
- `pythonnet>=3.0.5` - Python.NET with Python 3.12+ support
- `clr-loader>=0.2.6` - .NET Core runtime loader

## Documentation

See [PYTHON_INTEGRATION.md](PYTHON_INTEGRATION.md) for complete API documentation, troubleshooting, and advanced usage.

## Testing

Run integration tests:
```bash
python pybinding\test_python_integration.py
```

## Notes

- The Python wrapper automatically loads the C# DLL from `../bin/Release/net8.0/`
- Make sure to build the C# project before using Python bindings
- All datetime conversions between Python and C# are handled automatically
- The bindings use pythonnet to call C# code directly (no REST API or JSON serialization overhead)

