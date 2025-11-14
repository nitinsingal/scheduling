"""
Python wrapper for Scheduling C# library.
Provides access to Product, Location, and ProductLocation models.
"""

import os
import sys
from datetime import datetime
from pathlib import Path

# Configure pythonnet for .NET runtime
import clr_loader
from pythonnet import set_runtime

# Determine build configuration (Debug or Release)
# Set DOTNET_BUILD_CONFIG=Debug environment variable to load Debug DLL for C# debugging
build_config = os.environ.get("DOTNET_BUILD_CONFIG", "Release")
print(f"Loading .NET assemblies from {build_config} build...")

# Enable .NET debugging when in Debug mode
if build_config == "Debug":
    # These environment variables enable debugging support in CoreCLR
    os.environ["COMPlus_ZapDisable"] = "1"  # Disable NGEN/ReadyToRun for better debugging
    os.environ["COMPlus_ReadyToRun"] = "0"  # Disable ReadyToRun compilation
    os.environ["DOTNET_JitOptimize"] = "0"  # Disable JIT optimizations
    print("DEBUG: Enabled .NET debugging support (JIT optimizations disabled)")

# Get the directory containing the DLL (go up one level from pybinding to root)
dll_dir = Path(__file__).parent.parent / "bin" / build_config / "net8.0"
dll_path = dll_dir / "Scheduling.dll"


if not dll_path.exists():
    raise FileNotFoundError(
        f"Could not find Scheduling.dll at {dll_path}. "
        f"Please build the project first using: dotnet build -c {build_config}"
    )

# Set up the .NET runtime
runtime_config = dll_dir / "Scheduling.runtimeconfig.json"
rt = clr_loader.get_coreclr(runtime_config=str(runtime_config))
set_runtime(rt)

import clr
clr.AddReference(str(dll_path))

# Import C# namespaces
from Scheduling.Models import Product as CSharpProduct
from Scheduling.Models import Location as CSharpLocation
from Scheduling.Models import ProductLocation as CSharpProductLocation
from Scheduling.Models import DebugHelper as CSharpDebugHelper
from System import DateTime as CSharpDateTime


class DebugHelper:
    """
    Helper class for debugging C# code when called from Python.
    Use this to attach the .NET debugger at any point in your Python code.
    """
    
    @staticmethod
    def wait_for_debugger(timeout_seconds: int = 120, show_dots: bool = True) -> bool:
        """
        Pause execution and wait for a .NET debugger to attach.
        
        Args:
            timeout_seconds: Maximum time to wait (default: 120 seconds)
            show_dots: Whether to show progress dots (default: True)
            
        Returns:
            True if debugger attached, False if timeout
            
        Example:
            import scheduling_py as sched
            
            # Pause here and wait for debugger
            if sched.DebugHelper.wait_for_debugger(timeout_seconds=60):
                print("Debugger attached! Continue...")
            
            # Now call C# code - breakpoints will hit
            product = sched.Product.create("Widget")
        """
        return CSharpDebugHelper.WaitForDebugger(timeout_seconds, show_dots)
    
    @staticmethod
    def break_here():
        """
        Trigger a breakpoint if debugger is attached.
        If no debugger is attached, this does nothing.
        
        Example:
            import scheduling_py as sched
            
            sched.DebugHelper.wait_for_debugger()
            sched.DebugHelper.break_here()  # Will break here if debugger attached
        """
        CSharpDebugHelper.Break()
    
    @staticmethod
    def is_debugger_attached() -> bool:
        """
        Check if a .NET debugger is currently attached.
        
        Returns:
            True if debugger is attached, False otherwise
        """
        return CSharpDebugHelper.IsDebuggerAttached()
    
    @staticmethod
    def launch_debugger() -> bool:
        """
        Attempt to launch the Just-In-Time debugger dialog.
        Note: This may not work when .NET is embedded in Python.
        
        Returns:
            True if debugger was launched, False otherwise
        """
        return CSharpDebugHelper.LaunchDebugger()


class Product:
    """Python wrapper for C# Product class."""
    
    @staticmethod
    def create(name: str):
        """Create a new product or get existing one."""
        return CSharpProduct.Create(name)
    
    @staticmethod
    def get(name: str):
        """Get a product by name."""
        return CSharpProduct.Get(name)
    
    @staticmethod
    def remove(name: str) -> bool:
        """Remove a product by name."""
        return CSharpProduct.Remove(name)
    
    @staticmethod
    def exists(name: str) -> bool:
        """Check if a product exists."""
        return CSharpProduct.Exists(name)
    
    @staticmethod
    def get_all():
        """Get all products."""
        return list(CSharpProduct.GetAll())


class Location:
    """Python wrapper for C# Location class."""
    
    @staticmethod
    def create(name: str):
        """Create a new location or get existing one."""
        return CSharpLocation.Create(name)
    
    @staticmethod
    def get(name: str):
        """Get a location by name."""
        return CSharpLocation.Get(name)
    
    @staticmethod
    def remove(name: str) -> bool:
        """Remove a location by name."""
        return CSharpLocation.Remove(name)
    
    @staticmethod
    def exists(name: str) -> bool:
        """Check if a location exists."""
        return CSharpLocation.Exists(name)
    
    @staticmethod
    def get_all():
        """Get all locations."""
        return list(CSharpLocation.GetAll())


class ProductLocation:
    """Python wrapper for C# ProductLocation class."""
    
    @staticmethod
    def create(product_name: str, location_name: str):
        """Create a new ProductLocation or get existing one."""
        return CSharpProductLocation.Create(product_name, location_name)
    
    @staticmethod
    def get(product_name: str, location_name: str):
        """Get a ProductLocation by product and location names."""
        return CSharpProductLocation.Get(product_name, location_name)
    
    @staticmethod
    def get_by_key(key: str):
        """Get a ProductLocation by key (format: 'product@location')."""
        return CSharpProductLocation.GetByKey(key)
    
    @staticmethod
    def remove(product_name: str, location_name: str) -> bool:
        """Remove a ProductLocation."""
        return CSharpProductLocation.Remove(product_name, location_name)
    
    @staticmethod
    def exists(product_name: str, location_name: str) -> bool:
        """Check if a ProductLocation exists."""
        return CSharpProductLocation.Exists(product_name, location_name)
    
    @staticmethod
    def get_all():
        """Get all ProductLocations."""
        return list(CSharpProductLocation.GetAll())
    
    @staticmethod
    def get_by_product(product_name: str):
        """Get all ProductLocations for a specific product."""
        return list(CSharpProductLocation.GetByProduct(product_name))
    
    @staticmethod
    def get_by_location(location_name: str):
        """Get all ProductLocations for a specific location."""
        return list(CSharpProductLocation.GetByLocation(location_name))


def python_datetime_to_csharp(dt: datetime) -> CSharpDateTime:
    """Convert Python datetime to C# DateTime."""
    return CSharpDateTime(dt.year, dt.month, dt.day, dt.hour, dt.minute, dt.second, dt.microsecond // 1000)


def csharp_datetime_to_python(dt: CSharpDateTime) -> datetime:
    """Convert C# DateTime to Python datetime."""
    return datetime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond * 1000)


# Helper functions for inventory management
def add_inventory(product_location, time: datetime, quantity: float):
    """Add inventory to a ProductLocation at a specific time."""
    cs_time = python_datetime_to_csharp(time)
    product_location.AddInventory(cs_time, quantity)


def remove_inventory(product_location, time: datetime, quantity: float):
    """Remove inventory from a ProductLocation at a specific time."""
    cs_time = python_datetime_to_csharp(time)
    product_location.RemoveInventory(cs_time, quantity)


def update_inventory(product_location, time: datetime, net_change: float):
    """Update inventory change at a specific time."""
    cs_time = python_datetime_to_csharp(time)
    product_location.UpdateInventory(cs_time, net_change)


def get_cumulative_inventory(product_location, time: datetime) -> float:
    """Get cumulative inventory at a specific time."""
    cs_time = python_datetime_to_csharp(time)
    return product_location.GetCumulativeInventory(cs_time)


def get_inventory_change_at_time(product_location, time: datetime) -> float:
    """Get inventory change at a specific time."""
    cs_time = python_datetime_to_csharp(time)
    return product_location.GetInventoryChangeAtTime(cs_time)


def get_all_inventory_changes(product_location):
    """Get all inventory changes for a ProductLocation."""
    changes = product_location.GetAllInventoryChanges()
    return [(csharp_datetime_to_python(item.Item1), item.Item2) for item in changes]


def get_inventory_changes_in_range(product_location, start_time: datetime, end_time: datetime):
    """Get inventory changes within a time range."""
    cs_start = python_datetime_to_csharp(start_time)
    cs_end = python_datetime_to_csharp(end_time)
    changes = product_location.GetInventoryChangesInRange(cs_start, cs_end)
    return [(csharp_datetime_to_python(item.Item1), item.Item2) for item in changes]

