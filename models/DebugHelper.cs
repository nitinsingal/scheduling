using System;
using System.Diagnostics;
using System.Threading;

namespace Scheduling.Models
{
    /// <summary>
    /// Helper class for debugging C# code when called from Python.
    /// </summary>
    public static class DebugHelper
    {
        /// <summary>
        /// Waits for a debugger to attach. This is useful when calling C# from Python
        /// and you want to debug the C# code.
        /// </summary>
        /// <param name="timeoutSeconds">Maximum time to wait for debugger attachment (default: 120 seconds)</param>
        /// <param name="showDots">Whether to show dots while waiting (default: true)</param>
        /// <returns>True if debugger attached, false if timeout</returns>
        public static bool WaitForDebugger(int timeoutSeconds = 120, bool showDots = true)
        {
            if (Debugger.IsAttached)
            {
                Console.WriteLine("[DebugHelper] Debugger already attached.");
                return true;
            }

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("[DebugHelper] Waiting for .NET debugger to attach...");
            Console.WriteLine($"[DebugHelper] Process ID: {Environment.ProcessId}");
            Console.WriteLine($"[DebugHelper] Process Name: {Process.GetCurrentProcess().ProcessName}");
            Console.WriteLine($"[DebugHelper] Timeout: {timeoutSeconds} seconds");
            Console.WriteLine(new string('=', 60));
            Console.WriteLine("\nTo attach debugger:");
            Console.WriteLine("  1. In VS Code, select '.NET Core Attach' from Run & Debug");
            Console.WriteLine("  2. Press F5");
            Console.WriteLine($"  3. Search for process ID: {Environment.ProcessId}");
            Console.WriteLine("  4. Select the python.exe process");
            Console.WriteLine("  5. Set your C# breakpoints");
            Console.WriteLine("\nWaiting");

            // Wait for debugger attachment
            for (int i = 0; i < timeoutSeconds && !Debugger.IsAttached; i++)
            {
                Thread.Sleep(1000);
                if (showDots)
                {
                    Console.Write(".");
                    if ((i + 1) % 60 == 0)
                    {
                        Console.WriteLine($" {i + 1}s");
                    }
                }
            }

            Console.WriteLine();

            if (Debugger.IsAttached)
            {
                Console.WriteLine("\n✓ Debugger attached successfully!");
                Console.WriteLine(new string('=', 60) + "\n");
                return true;
            }
            else
            {
                Console.WriteLine("\n✗ Timeout: No debugger attached.");
                Console.WriteLine(new string('=', 60) + "\n");
                return false;
            }
        }

        /// <summary>
        /// Triggers a breakpoint if a debugger is attached.
        /// If no debugger is attached, this does nothing.
        /// </summary>
        public static void Break()
        {
            if (Debugger.IsAttached)
            {
                Console.WriteLine("[DebugHelper] Triggering breakpoint...");
                Debugger.Break();
            }
            else
            {
                Console.WriteLine("[DebugHelper] Break() called but no debugger attached.");
            }
        }

        /// <summary>
        /// Checks if a debugger is currently attached.
        /// </summary>
        /// <returns>True if debugger is attached, false otherwise</returns>
        public static bool IsDebuggerAttached()
        {
            return Debugger.IsAttached;
        }

        /// <summary>
        /// Attempts to launch the Just-In-Time debugger dialog.
        /// Note: This may not work when .NET is embedded in Python.
        /// </summary>
        /// <returns>True if debugger was launched, false otherwise</returns>
        public static bool LaunchDebugger()
        {
            if (Debugger.IsAttached)
            {
                Console.WriteLine("[DebugHelper] Debugger already attached.");
                return true;
            }

            Console.WriteLine("[DebugHelper] Attempting to launch debugger...");
            return Debugger.Launch();
        }
    }
}

