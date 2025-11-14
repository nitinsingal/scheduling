"""
Debug utilities for Python + C# debugging.
Provides helper functions to attach both Python and C# debuggers.
"""

import scheduling_py as sched


def python_debug_attach(port: int = 5678):
    """
    Attach Python debugger using debugpy.
    
    This will pause execution and wait for a Python debugger to attach
    on the specified port.
    
    Args:
        port: Port to listen on for debugger connection (default: 5678)
        
    Usage:
        1. Call this function in your Python code
        2. In VS Code, select "Python Debug" (or equivalent attach config)
        3. Press F5 to attach
        4. Put a breakpoint on the line after this function returns
        
    Example:
        from debug_utils import python_debug_attach
        
        python_debug_attach()
        # Execution continues here after debugger attaches
    """
    try:
        import debugpy
        debugpy.listen(port)
        print(f"\n[Python Debug] Waiting for Python debugger on port {port}...")
        debugpy.wait_for_client()
        print("[Python Debug] Python debugger attached!")
    except ImportError:
        print("[Python Debug] ERROR: debugpy not installed. Install with: pip install debugpy")
    except Exception as e:
        print(f"[Python Debug] ERROR: Failed to attach debugger: {e}")


def csharp_debug_attach(timeout_seconds: int = 60, show_dots: bool = True):
    """
    Wait for C# debugger to attach.
    
    This will pause execution and wait for a .NET debugger to attach
    to the Python process (which is hosting the .NET runtime).
    
    Args:
        timeout_seconds: Maximum time to wait for debugger (default: 60 seconds)
        show_dots: Show progress dots while waiting (default: True)
        
    Returns:
        True if debugger attached, False if timeout
        
    Usage:
        1. Call this function before executing C# code
        2. In VS Code, select ".NET Core Attach" from Run & Debug
        3. Press F5 and search for the Python process ID (shown in output)
        4. Select the python.exe process
        5. Set breakpoints in your C# files
        6. The function returns and C# code executes with breakpoints active
        
    Example:
        from debug_utils import csharp_debug_attach
        import scheduling_py as sched
        
        # Wait for C# debugger
        if csharp_debug_attach(timeout_seconds=60):
            print("C# debugger ready!")
        
        # Now C# breakpoints will hit
        product = sched.Product.create("Widget")
    """
    print("\n" + "=" * 60)
    print("[C# Debug] Waiting for .NET Debugger")
    print("=" * 60)
    
    attached = sched.DebugHelper.wait_for_debugger(
        timeout_seconds=timeout_seconds,
        show_dots=show_dots
    )
    
    if attached:
        print("[C# Debug] ✓ C# Debugger attached! C# breakpoints will now hit.\n")
    else:
        print("[C# Debug] ✗ No debugger attached. Continuing without C# debugging...\n")
    
    return attached


def check_csharp_debugger() -> bool:
    """
    Check if a C# debugger is currently attached.
    
    Returns:
        True if C# debugger is attached, False otherwise
        
    Example:
        from debug_utils import check_csharp_debugger
        
        if check_csharp_debugger():
            print("C# debugger is active")
    """
    return sched.DebugHelper.is_debugger_attached()


def csharp_break():
    """
    Trigger a breakpoint in C# if debugger is attached.
    
    This is useful when you want to break at a specific point in your
    Python code before calling C# methods.
    
    If no debugger is attached, this does nothing.
    
    Example:
        from debug_utils import csharp_debug_attach, csharp_break
        import scheduling_py as sched
        
        csharp_debug_attach()
        
        # ... some Python code ...
        
        csharp_break()  # Break here before calling C#
        product = sched.Product.create("Widget")
    """
    sched.DebugHelper.break_here()


def setup_dual_debugging(python_port: int = 5678, csharp_timeout: int = 60):
    """
    Setup both Python and C# debugging in one call.
    
    This is a convenience function that:
    1. Waits for Python debugger to attach
    2. Then waits for C# debugger to attach
    
    Args:
        python_port: Port for Python debugger (default: 5678)
        csharp_timeout: Timeout for C# debugger (default: 60 seconds)
        
    Returns:
        Tuple of (python_attached: bool, csharp_attached: bool)
        
    Usage:
        1. Run your Python script
        2. Attach Python debugger first
        3. Then attach C# debugger
        4. Both debuggers will be active
        
    Example:
        from debug_utils import setup_dual_debugging
        
        python_ok, csharp_ok = setup_dual_debugging()
        
        if python_ok and csharp_ok:
            print("Both debuggers ready!")
        
        # Now you can debug both Python and C# code
    """
    print("\n" + "=" * 70)
    print("DUAL DEBUGGING SETUP (Python + C#)")
    print("=" * 70)
    
    # Step 1: Python debugger
    print("\nStep 1/2: Attaching Python debugger...")
    python_debug_attach(port=python_port)
    python_attached = True  # If we get here, Python debugger attached
    
    # Step 2: C# debugger
    print("\nStep 2/2: Attaching C# debugger...")
    csharp_attached = csharp_debug_attach(timeout_seconds=csharp_timeout)
    
    print("=" * 70)
    print(f"Python debugger: {'✓ Attached' if python_attached else '✗ Not attached'}")
    print(f"C# debugger: {'✓ Attached' if csharp_attached else '✗ Not attached'}")
    print("=" * 70 + "\n")
    
    return python_attached, csharp_attached

