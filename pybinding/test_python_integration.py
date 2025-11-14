"""
Quick test to verify Python integration with C# models.
"""

import scheduling_py as sched
from datetime import datetime

def test_basic_operations():
    """Test basic Product, Location, and ProductLocation operations."""
    
    print("Testing Python Integration...")
    
    # Test Product
    print("\n1. Testing Product...")
    p = sched.Product.create("TestProduct")
    assert p.Name == "TestProduct"
    assert sched.Product.exists("TestProduct")
    print("   ✓ Product creation works!")
    
    # Test Location
    print("\n2. Testing Location...")
    l = sched.Location.create("TestLocation")
    assert l.Name == "TestLocation"
    assert sched.Location.exists("TestLocation")
    print("   ✓ Location creation works!")
    
    # Test ProductLocation
    print("\n3. Testing ProductLocation...")
    pl = sched.ProductLocation.create("TestProduct", "TestLocation")
    assert pl.Key == "TestProduct@TestLocation"
    assert pl.ProductName == "TestProduct"
    assert pl.LocationName == "TestLocation"
    print("   ✓ ProductLocation creation works!")
    
    # Test Inventory
    print("\n4. Testing Inventory...")
    test_time = datetime(2024, 1, 1, 12, 0, 0)
    
    sched.add_inventory(pl, test_time, 100)
    cumulative = sched.get_cumulative_inventory(pl, test_time)
    assert cumulative == 100
    print(f"   ✓ Add inventory works! Cumulative: {cumulative}")
    
    sched.remove_inventory(pl, test_time, 30)
    cumulative = sched.get_cumulative_inventory(pl, test_time)
    assert cumulative == 70
    print(f"   ✓ Remove inventory works! Cumulative: {cumulative}")
    
    print("\n✅ All tests passed!")
    print("\nYou can now use the C# models from Python!")
    print("Run 'python example_usage.py' to see a full example.")

if __name__ == "__main__":
    try:
        test_basic_operations()
    except Exception as e:
        print(f"\n❌ Error: {e}")
        print("\nMake sure:")
        print("  1. Virtual environment is activated")
        print("  2. pythonnet is installed: pip install pythonnet")
        print("  3. C# project is built: dotnet build --configuration Release")

