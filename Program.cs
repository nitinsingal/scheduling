using System;

namespace Scheduling
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting debug test...");

            int x = 10;
            int y = 20;
            int z = x + y;  // SET BREAKPOINT HERE

            Console.WriteLine($"Result: {z}");
            Console.WriteLine("Debug test complete!");
        }
    }
}

