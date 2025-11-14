using Scheduling.Models;

namespace Scheduling.Tests
{
    /// <summary>
    /// Base class for all tests that automatically resets all static state before and after each test.
    /// xUnit automatically calls Dispose() after each test method completes.
    /// </summary>
    public abstract class TestBase : IDisposable
    {
        /// <summary>
        /// Resets all static state (Operations, Products, Locations, ProductLocations).
        /// Called automatically before and after each test.
        /// </summary>
        public static void Reset()
        {
            Operation.Clear();
            Product.Clear();
            Location.Clear();
            ProductLocation.Clear();
        }

        /// <summary>
        /// Constructor - called before each test. Resets all static state.
        /// </summary>
        protected TestBase()
        {
            Reset();
        }

        /// <summary>
        /// Called automatically by xUnit after each test completes.
        /// This ensures cleanup happens after every test method, even if the test throws an exception.
        /// </summary>
        public void Dispose()
        {
            Reset();
        }
    }
}

