using System.Diagnostics;
using Scheduling.Tests;


// Use the folowing command to launch the test in powershell after creating a debug build
// dotnet test Scheduling.Tests\Scheduling.Tests.csproj --filter 'FullyQualifiedName=TestDriver.DebugTest' --configuration Debug --no-build --logger 'console;verbosity=detailed'
public class TestDriver
{
    [Fact]
    public void DebugTest()
    {
        // Wait for debugger to attach if not already attached
        if (!Debugger.IsAttached)
        {
            Console.WriteLine("Waiting for the debugger to attach...");
            Console.WriteLine($"Process ID: {Environment.ProcessId}");

            // Give user 120 seconds to attach debugger manually
            for (int i = 0; i < 60 && !Debugger.IsAttached; i++)
            {
                Thread.Sleep(1000);
                Console.Write(".");
            }
            Console.WriteLine();
        }

        if (Debugger.IsAttached)
        {
            Debugger.Break(); // This will break into the debugger
        }

        Console.WriteLine("Test1 executing...");
        // Invoke a test here. For example from ProductLocationTests.cs
        var productLocationTests = new ProductLocationTests();
        productLocationTests.Create_WithValidNames_ShouldCreateProductLocation();


    }
}