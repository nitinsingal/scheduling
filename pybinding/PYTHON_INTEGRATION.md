# Python Integration for Scheduling C# Library

This guide shows how to use the Scheduling C# models from Python.

## Setup

### 1. Prerequisites
- Python 3.8+ installed
- .NET 8.0 SDK installed
- Virtual environment activated

### 2. Install Dependencies

```powershell
# Make sure virtual environment is activated
pip install -r requirements.txt
```

### 3. Build the C# Project

```powershell
dotnet build Scheduling.csproj --configuration Release
```

## Quick Start

### Test the Integration

```powershell
python test_python_integration.py
```

### Run the Full Example

```powershell
python example_usage.py
```

## Usage

### Import the Module

```python
import scheduling_py as sched
from datetime import datetime
```

### Working with Products

```python
# Create a product
product = sched.Product.create("Widget")

# Get a product
product = sched.Product.get("Widget")

# Check if exists
exists = sched.Product.exists("Widget")  # Returns True/False

# Get all products
all_products = sched.Product.get_all()
for p in all_products:
    print(p.Name)

# Remove a product
sched.Product.remove("Widget")
```

### Working with Locations

```python
# Create a location
location = sched.Location.create("Warehouse-A")

# Get a location
location = sched.Location.get("Warehouse-A")

# Check if exists
exists = sched.Location.exists("Warehouse-A")  # Returns True/False

# Get all locations
all_locations = sched.Location.get_all()
for l in all_locations:
    print(l.Name)

# Remove a location
sched.Location.remove("Warehouse-A")
```

### Working with ProductLocations

```python
# Create a ProductLocation (auto-creates Product and Location if needed)
pl = sched.ProductLocation.create("Widget", "Warehouse-A")

# Get a ProductLocation
pl = sched.ProductLocation.get("Widget", "Warehouse-A")

# Get by key
pl = sched.ProductLocation.get_by_key("Widget@Warehouse-A")

# Check if exists
exists = sched.ProductLocation.exists("Widget", "Warehouse-A")

# Get all ProductLocations
all_pls = sched.ProductLocation.get_all()

# Get ProductLocations for a specific product
widget_locations = sched.ProductLocation.get_by_product("Widget")

# Get ProductLocations for a specific location
warehouse_products = sched.ProductLocation.get_by_location("Warehouse-A")

# Remove a ProductLocation
sched.ProductLocation.remove("Widget", "Warehouse-A")
```

### Managing Inventory

```python
from datetime import datetime, timedelta

# Create a ProductLocation
pl = sched.ProductLocation.create("Widget", "Warehouse-A")

# Add inventory at a specific time
time1 = datetime(2024, 1, 1, 8, 0, 0)
sched.add_inventory(pl, time1, 100)  # Add 100 units

# Remove inventory
time2 = time1 + timedelta(hours=2)
sched.remove_inventory(pl, time2, 30)  # Remove 30 units

# Update inventory (replace existing change at a time)
sched.update_inventory(pl, time1, 150)  # Change to 150 units instead of 100

# Get cumulative inventory at a specific time
query_time = time1 + timedelta(hours=1)
cumulative = sched.get_cumulative_inventory(pl, query_time)
print(f"Inventory at {query_time}: {cumulative} units")

# Get inventory change at a specific time
change = sched.get_inventory_change_at_time(pl, time1)
print(f"Change at {time1}: {change} units")

# Get all inventory changes
all_changes = sched.get_all_inventory_changes(pl)
for time, net_change in all_changes:
    print(f"{time}: {net_change:+.2f} units")

# Get inventory changes in a time range
start = time1
end = time1 + timedelta(hours=4)
range_changes = sched.get_inventory_changes_in_range(pl, start, end)
for time, net_change in range_changes:
    print(f"{time}: {net_change:+.2f} units")
```

## API Reference

### Product Class

| Method | Description | Returns |
|--------|-------------|---------|
| `create(name)` | Create or get a product | Product object |
| `get(name)` | Get a product by name | Product object or None |
| `remove(name)` | Remove a product | bool |
| `exists(name)` | Check if product exists | bool |
| `get_all()` | Get all products | List of Product objects |

### Location Class

| Method | Description | Returns |
|--------|-------------|---------|
| `create(name)` | Create or get a location | Location object |
| `get(name)` | Get a location by name | Location object or None |
| `remove(name)` | Remove a location | bool |
| `exists(name)` | Check if location exists | bool |
| `get_all()` | Get all locations | List of Location objects |

### ProductLocation Class

| Method | Description | Returns |
|--------|-------------|---------|
| `create(product_name, location_name)` | Create or get a ProductLocation | ProductLocation object |
| `get(product_name, location_name)` | Get a ProductLocation | ProductLocation object or None |
| `get_by_key(key)` | Get by key "product@location" | ProductLocation object or None |
| `remove(product_name, location_name)` | Remove a ProductLocation | bool |
| `exists(product_name, location_name)` | Check if exists | bool |
| `get_all()` | Get all ProductLocations | List of ProductLocation objects |
| `get_by_product(product_name)` | Get all for a product | List of ProductLocation objects |
| `get_by_location(location_name)` | Get all for a location | List of ProductLocation objects |

### Inventory Helper Functions

| Function | Description | Parameters | Returns |
|----------|-------------|------------|---------|
| `add_inventory(pl, time, quantity)` | Add inventory | ProductLocation, datetime, float | None |
| `remove_inventory(pl, time, quantity)` | Remove inventory | ProductLocation, datetime, float | None |
| `update_inventory(pl, time, net_change)` | Update inventory change | ProductLocation, datetime, float | None |
| `get_cumulative_inventory(pl, time)` | Get cumulative inventory | ProductLocation, datetime | float |
| `get_inventory_change_at_time(pl, time)` | Get change at time | ProductLocation, datetime | float |
| `get_all_inventory_changes(pl)` | Get all changes | ProductLocation | List of (datetime, float) tuples |
| `get_inventory_changes_in_range(pl, start, end)` | Get changes in range | ProductLocation, datetime, datetime | List of (datetime, float) tuples |

## Notes

- All C# models use singleton pattern (same name = same object)
- ProductLocation automatically creates Product and Location if they don't exist
- DateTime conversion is handled automatically between Python and C#
- The C# DLL must be built before using the Python wrapper
- Changes made in Python are reflected in the C# models (shared state)

## Troubleshooting

### "Could not find Scheduling.dll"
Build the C# project:
```powershell
dotnet build Scheduling.csproj --configuration Release
```

### "No module named 'clr'"
Install pythonnet:
```powershell
pip install pythonnet
```

### Virtual Environment Not Found
Create and activate:
```powershell
python -m venv venv
.\venv\Scripts\Activate.ps1
```

