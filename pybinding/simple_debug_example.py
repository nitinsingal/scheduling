"""
Simple example showing different ways to use debug_utils.
"""

import scheduling_py as sched
from debug_utils import csharp_debug_attach, csharp_break, setup_dual_debugging


def example1_csharp_only():
    """Debug C# code only (no Python debugger)."""
    print("\n" + "="*60)
    print("EXAMPLE 1: C# Debugging Only")
    print("="*60)
    
    # Wait for C# debugger to attach
    csharp_debug_attach(timeout_seconds=30)
    
    # Set breakpoint in Product.cs line 28 before running this
    print("Creating product (C# breakpoint should hit)...")
    product = sched.Product.create("Widget")
    print(f"Created: {product.Name}")


def example2_dual_debugging():
    """Debug both Python and C# code."""
    print("\n" + "="*60)
    print("EXAMPLE 2: Dual Debugging (Python + C#)")
    print("="*60)
    
    # Setup both debuggers at once
    python_ok, csharp_ok = setup_dual_debugging(
        python_port=5678,
        csharp_timeout=30
    )
    
    # Put a Python breakpoint here
    print("Both debuggers ready!")
    
    # C# breakpoint will also hit
    product = sched.Product.create("Gadget")
    print(f"Created: {product.Name}")


def example3_manual_breaks():
    """Use manual breakpoints in C# code."""
    print("\n" + "="*60)
    print("EXAMPLE 3: Manual C# Breakpoints")
    print("="*60)
    
    csharp_debug_attach(timeout_seconds=30)
    
    print("Creating first product...")
    product1 = sched.Product.create("Widget")
    
    # Trigger a breakpoint before next C# call
    print("\nAbout to create second product...")
    csharp_break()  # This will break HERE if C# debugger is attached
    
    product2 = sched.Product.create("Gadget")
    print(f"Created: {product1.Name} and {product2.Name}")


if __name__ == "__main__":
    import sys
    
    print("Choose an example:")
    print("  1 - C# debugging only")
    print("  2 - Dual debugging (Python + C#)")
    print("  3 - Manual C# breakpoints")
    
    if len(sys.argv) > 1:
        choice = sys.argv[1]
    else:
        choice = input("\nEnter choice (1-3): ").strip()
    
    if choice == "1":
        example1_csharp_only()
    elif choice == "2":
        example2_dual_debugging()
    elif choice == "3":
        example3_manual_breaks()
    else:
        print("Invalid choice. Running example 1...")
        example1_csharp_only()

