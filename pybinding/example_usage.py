"""
Example usage of the Scheduling C# library from Python.
"""

from datetime import datetime, timedelta
import scheduling_py as sched
from debug_utils import python_debug_attach, csharp_debug_attach

def main():
    print("=" * 60)
    print("Scheduling C# Library - Python Integration Example")
    print("=" * 60)
    
    python_debug_attach()

    # Within 60 seconds attach the .Net debugger. No need to put a breakpoint here in python file.
    # There should be a breakpoint in the C# code that is waiting for the debugger to attach.
    csharp_debug_attach(60)
    

    print("\n--- Creating Products ---")

    product1 = sched.Product.create("Widget")
    product2 = sched.Product.create("Gadget")
    product3 = sched.Product.create("Doohickey")
    
    print(f"Created: {product1.Name}")
    print(f"Created: {product2.Name}")
    print(f"Created: {product3.Name}")
    
    # Check if product exists
    print(f"\nProduct 'Widget' exists: {sched.Product.exists('Widget')}")
    print(f"Product 'NonExistent' exists: {sched.Product.exists('NonExistent')}")
    
    # Get all products
    print(f"\nAll products:")
    for product in sched.Product.get_all():
        print(f"  - {product.Name}")
    
    # ==================== Location Operations ====================
    print("\n--- Creating Locations ---")
    location1 = sched.Location.create("Warehouse-A")
    location2 = sched.Location.create("Warehouse-B")
    location3 = sched.Location.create("Store-1")
    
    print(f"Created: {location1.Name}")
    print(f"Created: {location2.Name}")
    print(f"Created: {location3.Name}")
    
    # Get all locations
    print(f"\nAll locations:")
    for location in sched.Location.get_all():
        print(f"  - {location.Name}")
    
    # ==================== ProductLocation Operations ====================
    print("\n--- Creating ProductLocations ---")
    pl1 = sched.ProductLocation.create("Widget", "Warehouse-A")
    pl2 = sched.ProductLocation.create("Widget", "Warehouse-B")
    pl3 = sched.ProductLocation.create("Gadget", "Warehouse-A")
    pl4 = sched.ProductLocation.create("Gadget", "Store-1")
    
    print(f"Created: {pl1.Key}")
    print(f"Created: {pl2.Key}")
    print(f"Created: {pl3.Key}")
    print(f"Created: {pl4.Key}")
    
    # Get ProductLocations by product
    print(f"\nAll ProductLocations for 'Widget':")
    for pl in sched.ProductLocation.get_by_product("Widget"):
        print(f"  - {pl.Key}")
    
    # Get ProductLocations by location
    print(f"\nAll ProductLocations for 'Warehouse-A':")
    for pl in sched.ProductLocation.get_by_location("Warehouse-A"):
        print(f"  - {pl.Key}")
    
    # ==================== Inventory Operations ====================
    print("\n--- Inventory Management ---")
    
    # Add inventory over time
    base_time = datetime(2024, 1, 1, 8, 0, 0)
    
    print(f"\nAdding inventory to {pl1.Key}:")
    sched.add_inventory(pl1, base_time, 100)
    print(f"  {base_time}: +100 units")
    
    sched.add_inventory(pl1, base_time + timedelta(hours=2), 50)
    print(f"  {base_time + timedelta(hours=2)}: +50 units")
    
    sched.remove_inventory(pl1, base_time + timedelta(hours=4), 30)
    print(f"  {base_time + timedelta(hours=4)}: -30 units")
    
    sched.add_inventory(pl1, base_time + timedelta(hours=6), 75)
    print(f"  {base_time + timedelta(hours=6)}: +75 units")
    
    # Get cumulative inventory at different times
    print(f"\nCumulative inventory levels:")
    query_times = [
        base_time + timedelta(hours=1),
        base_time + timedelta(hours=3),
        base_time + timedelta(hours=5),
        base_time + timedelta(hours=7)
    ]
    
    for qt in query_times:
        cumulative = sched.get_cumulative_inventory(pl1, qt)
        print(f"  At {qt}: {cumulative} units")
    
    # Get all inventory changes
    print(f"\nAll inventory changes for {pl1.Key}:")
    all_changes = sched.get_all_inventory_changes(pl1)
    for time, net_change in all_changes:
        sign = "+" if net_change >= 0 else ""
        print(f"  {time}: {sign}{net_change} units")
    
    # Get inventory changes in a specific range
    start = base_time + timedelta(hours=1)
    end = base_time + timedelta(hours=5)
    print(f"\nInventory changes between {start} and {end}:")
    range_changes = sched.get_inventory_changes_in_range(pl1, start, end)
    for time, net_change in range_changes:
        sign = "+" if net_change >= 0 else ""
        print(f"  {time}: {sign}{net_change} units")
    
    # ==================== Update Inventory ====================
    print("\n--- Updating Inventory ---")
    update_time = base_time + timedelta(hours=2)
    print(f"Original change at {update_time}: {sched.get_inventory_change_at_time(pl1, update_time)} units")
    
    sched.update_inventory(pl1, update_time, 80)  # Update to 80 instead of 50
    print(f"Updated change at {update_time}: {sched.get_inventory_change_at_time(pl1, update_time)} units")
    
    cumulative_after_update = sched.get_cumulative_inventory(pl1, base_time + timedelta(hours=7))
    print(f"New cumulative inventory at end: {cumulative_after_update} units")
    
    print("\n" + "=" * 60)
    print("Example completed successfully!")
    print("=" * 60)


if __name__ == "__main__":
    main()

