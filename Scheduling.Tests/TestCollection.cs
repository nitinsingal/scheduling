using Xunit;

namespace Scheduling.Tests
{
    /// <summary>
    /// Defines a single test collection to ensure all tests run serially.
    /// Tests in the same collection run sequentially, not in parallel.
    /// </summary>
    [CollectionDefinition("Serial")]
    public class TestCollection : ICollectionFixture<object>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // [Collection] attributes.
    }
}

