using System.Linq;
using Scheduling.Models;
using Xunit;

namespace Scheduling.Tests
{
    [Collection("Serial")]
    public class LocationTests : TestBase
    {
        [Fact]
        public void Create_WithValidName_ShouldCreateLocation()
        {
            // Arrange
            var locationName = "TestLocation";

            // Act
            var location = Location.Create(locationName);

            // Assert
            Assert.NotNull(location);
            Assert.Equal(locationName, location.Name);
        }

        [Fact]
        public void Create_WithSameName_ShouldReturnSameInstance()
        {
            // Arrange
            var locationName = "TestLocation";

            // Act
            var location1 = Location.Create(locationName);
            var location2 = Location.Create(locationName);

            // Assert
            Assert.Same(location1, location2);
        }

        [Fact]
        public void Create_WithNullName_ShouldThrowArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Location.Create(null!));
        }

        [Fact]
        public void Create_WithEmptyName_ShouldThrowArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Location.Create(""));
        }

        [Fact]
        public void Create_WithWhitespaceName_ShouldThrowArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Location.Create("   "));
        }

        [Fact]
        public void Get_WithExistingLocation_ShouldReturnLocation()
        {
            // Arrange
            var locationName = "ExistingLocation";
            Location.Create(locationName);

            // Act
            var location = Location.Get(locationName);

            // Assert
            Assert.NotNull(location);
            Assert.Equal(locationName, location!.Name);
        }

        [Fact]
        public void Get_WithNonExistingLocation_ShouldReturnNull()
        {
            // Act
            var location = Location.Get("NonExistingLocation");

            // Assert
            Assert.Null(location);
        }

        [Fact]
        public void Exists_WithExistingLocation_ShouldReturnTrue()
        {
            // Arrange
            var locationName = "ExistingLocation";
            Location.Create(locationName);

            // Act
            var exists = Location.Exists(locationName);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public void Exists_WithNonExistingLocation_ShouldReturnFalse()
        {
            // Act
            var exists = Location.Exists("NonExistingLocation");

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public void Remove_WithExistingLocation_ShouldReturnTrue()
        {
            // Arrange
            var locationName = "LocationToRemove";
            Location.Create(locationName);

            // Act
            var removed = Location.Remove(locationName);

            // Assert
            Assert.True(removed);
            Assert.False(Location.Exists(locationName));
        }

        [Fact]
        public void Remove_WithNonExistingLocation_ShouldReturnFalse()
        {
            // Act
            var removed = Location.Remove("NonExistingLocation");

            // Assert
            Assert.False(removed);
        }

        [Fact]
        public void GetAll_ShouldReturnAllLocations()
        {
            // Arrange
            Location.Create("Location1");
            Location.Create("Location2");
            Location.Create("Location3");

            // Act
            var locations = Location.GetAll().ToList();

            // Assert
            Assert.True(locations.Count >= 3);
            Assert.Contains(locations, l => l.Name == "Location1");
            Assert.Contains(locations, l => l.Name == "Location2");
            Assert.Contains(locations, l => l.Name == "Location3");
        }
    }
}

