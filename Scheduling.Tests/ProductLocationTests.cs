using System.Linq;
using Scheduling.Models;
using Xunit;

namespace Scheduling.Tests
{
    [Collection("Serial")]
    public class ProductLocationTests : TestBase
    {
        [Fact]
        public void Create_WithValidNames_ShouldCreateProductLocation()
        {
            // Arrange
            var productName = "TestProduct";
            var locationName = "TestLocation";

            // Act
            var productLocation = ProductLocation.Create(productName, locationName);

            // Assert
            Assert.NotNull(productLocation);
            Assert.Equal(productName, productLocation.ProductName);
            Assert.Equal(locationName, productLocation.LocationName);
            Assert.Equal($"{productName}@{locationName}", productLocation.Key);
        }

        [Fact]
        public void Create_WithNames_ShouldAutoCreateProductAndLocation()
        {
            // Arrange
            var productName = "AutoProduct";
            var locationName = "AutoLocation";

            // Act
            var productLocation = ProductLocation.Create(productName, locationName);

            // Assert
            Assert.True(Product.Exists(productName));
            Assert.True(Location.Exists(locationName));
            Assert.NotNull(productLocation.Product);
            Assert.NotNull(productLocation.Location);
        }

        [Fact]
        public void Create_WithProductAndLocationObjects_ShouldCreateProductLocation()
        {
            // Arrange
            var product = Product.Create("ObjectProduct");
            var location = Location.Create("ObjectLocation");

            // Act
            var productLocation = ProductLocation.Create(product, location);

            // Assert
            Assert.NotNull(productLocation);
            Assert.Equal(product.Name, productLocation.ProductName);
            Assert.Equal(location.Name, productLocation.LocationName);
            Assert.Same(product, productLocation.Product);
            Assert.Same(location, productLocation.Location);
        }

        [Fact]
        public void Create_WithNullProduct_ShouldThrowArgumentNullException()
        {
            // Arrange
            var location = Location.Create("TestLocation");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ProductLocation.Create(null!, location));
        }

        [Fact]
        public void Create_WithNullLocation_ShouldThrowArgumentNullException()
        {
            // Arrange
            var product = Product.Create("TestProduct");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ProductLocation.Create(product, null!));
        }

        [Fact]
        public void Create_WithNullProductName_ShouldThrowArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => ProductLocation.Create(null!, "Location"));
        }

        [Fact]
        public void Create_WithNullLocationName_ShouldThrowArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => ProductLocation.Create("Product", null!));
        }

        [Fact]
        public void Create_WithEmptyProductName_ShouldThrowArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => ProductLocation.Create("", "Location"));
        }

        [Fact]
        public void Create_WithEmptyLocationName_ShouldThrowArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => ProductLocation.Create("Product", ""));
        }

        [Fact]
        public void Create_WithSameNames_ShouldReturnSameInstance()
        {
            // Arrange
            var productName = "SameProduct";
            var locationName = "SameLocation";

            // Act
            var productLocation1 = ProductLocation.Create(productName, locationName);
            var productLocation2 = ProductLocation.Create(productName, locationName);

            // Assert
            Assert.Same(productLocation1, productLocation2);
        }

        [Fact]
        public void Create_WithSameProductAndLocationObjects_ShouldReturnSameInstance()
        {
            // Arrange
            var product = Product.Create("SameObjectProduct");
            var location = Location.Create("SameObjectLocation");

            // Act
            var productLocation1 = ProductLocation.Create(product, location);
            var productLocation2 = ProductLocation.Create(product, location);

            // Assert
            Assert.Same(productLocation1, productLocation2);
        }

        [Fact]
        public void Get_WithExistingProductLocation_ShouldReturnProductLocation()
        {
            // Arrange
            var productName = "GetProduct";
            var locationName = "GetLocation";
            ProductLocation.Create(productName, locationName);

            // Act
            var productLocation = ProductLocation.Get(productName, locationName);

            // Assert
            Assert.NotNull(productLocation);
            Assert.Equal(productName, productLocation!.ProductName);
            Assert.Equal(locationName, productLocation.LocationName);
        }

        [Fact]
        public void Get_WithNonExistingProductLocation_ShouldReturnNull()
        {
            // Act
            var productLocation = ProductLocation.Get("NonExistingProduct", "NonExistingLocation");

            // Assert
            Assert.Null(productLocation);
        }

        [Fact]
        public void GetByKey_WithValidKey_ShouldReturnProductLocation()
        {
            // Arrange
            var productName = "KeyProduct";
            var locationName = "KeyLocation";
            var key = $"{productName}@{locationName}";
            ProductLocation.Create(productName, locationName);

            // Act
            var productLocation = ProductLocation.GetByKey(key);

            // Assert
            Assert.NotNull(productLocation);
            Assert.Equal(key, productLocation!.Key);
        }

        [Fact]
        public void GetByKey_WithInvalidKey_ShouldReturnNull()
        {
            // Act
            var productLocation = ProductLocation.GetByKey("Invalid@Key");

            // Assert
            Assert.Null(productLocation);
        }

        [Fact]
        public void Exists_WithExistingProductLocation_ShouldReturnTrue()
        {
            // Arrange
            var productName = "ExistsProduct";
            var locationName = "ExistsLocation";
            ProductLocation.Create(productName, locationName);

            // Act
            var exists = ProductLocation.Exists(productName, locationName);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public void Exists_WithNonExistingProductLocation_ShouldReturnFalse()
        {
            // Act
            var exists = ProductLocation.Exists("NonExistingProduct", "NonExistingLocation");

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public void Remove_WithExistingProductLocation_ShouldReturnTrue()
        {
            // Arrange
            var productName = "RemoveProduct";
            var locationName = "RemoveLocation";
            ProductLocation.Create(productName, locationName);

            // Act
            var removed = ProductLocation.Remove(productName, locationName);

            // Assert
            Assert.True(removed);
            Assert.False(ProductLocation.Exists(productName, locationName));
        }

        [Fact]
        public void Remove_WithNonExistingProductLocation_ShouldReturnFalse()
        {
            // Act
            var removed = ProductLocation.Remove("NonExistingProduct", "NonExistingLocation");

            // Assert
            Assert.False(removed);
        }

        [Fact]
        public void GetAll_ShouldReturnAllProductLocations()
        {
            // Arrange
            ProductLocation.Create("Product1", "Location1");
            ProductLocation.Create("Product2", "Location2");
            ProductLocation.Create("Product3", "Location3");

            // Act
            var productLocations = ProductLocation.GetAll().ToList();

            // Assert
            Assert.True(productLocations.Count >= 3);
            Assert.Contains(productLocations, pl => pl.ProductName == "Product1" && pl.LocationName == "Location1");
            Assert.Contains(productLocations, pl => pl.ProductName == "Product2" && pl.LocationName == "Location2");
            Assert.Contains(productLocations, pl => pl.ProductName == "Product3" && pl.LocationName == "Location3");
        }

        [Fact]
        public void GetByProduct_ShouldReturnAllProductLocationsForProduct()
        {
            // Arrange
            var productName = "MultiLocationProduct";
            ProductLocation.Create(productName, "Location1");
            ProductLocation.Create(productName, "Location2");
            ProductLocation.Create(productName, "Location3");
            ProductLocation.Create("OtherProduct", "Location1"); // Different product

            // Act
            var productLocations = ProductLocation.GetByProduct(productName).ToList();

            // Assert
            Assert.True(productLocations.Count >= 3);
            Assert.All(productLocations, pl => Assert.Equal(productName, pl.ProductName));
        }

        [Fact]
        public void GetByLocation_ShouldReturnAllProductLocationsForLocation()
        {
            // Arrange
            var locationName = "MultiProductLocation";
            ProductLocation.Create("Product1", locationName);
            ProductLocation.Create("Product2", locationName);
            ProductLocation.Create("Product3", locationName);
            ProductLocation.Create("Product1", "OtherLocation"); // Different location

            // Act
            var productLocations = ProductLocation.GetByLocation(locationName).ToList();

            // Assert
            Assert.True(productLocations.Count >= 3);
            Assert.All(productLocations, pl => Assert.Equal(locationName, pl.LocationName));
        }

        [Fact]
        public void Create_WithNamesAndObjects_ShouldCreateSameProductLocation()
        {
            // Arrange
            var productName = "MixedProduct";
            var locationName = "MixedLocation";
            var product = Product.Create(productName);
            var location = Location.Create(locationName);

            // Act
            var productLocation1 = ProductLocation.Create(productName, locationName);
            var productLocation2 = ProductLocation.Create(product, location);

            // Assert
            Assert.Same(productLocation1, productLocation2);
            Assert.Equal($"{productName}@{locationName}", productLocation1.Key);
        }

        // Inventory Management Tests

        [Fact]
        public void AddInventory_WithValidQuantity_ShouldAddToInventoryProfile()
        {
            // Arrange
            var productLocation = ProductLocation.Create("InventoryProduct1", "InventoryLocation1");
            var time = new DateTime(2025, 1, 1, 10, 0, 0);
            var quantity = 100.0;

            // Act
            productLocation.AddInventory(time, quantity);

            // Assert
            var change = productLocation.GetInventoryChangeAtTime(time);
            Assert.Equal(quantity, change);
        }

        [Fact]
        public void AddInventory_WithZeroQuantity_ShouldThrowArgumentException()
        {
            // Arrange
            var productLocation = ProductLocation.Create("InventoryProduct2", "InventoryLocation2");
            var time = new DateTime(2025, 1, 1, 10, 0, 0);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => productLocation.AddInventory(time, 0));
        }

        [Fact]
        public void AddInventory_WithNegativeQuantity_ShouldThrowArgumentException()
        {
            // Arrange
            var productLocation = ProductLocation.Create("InventoryProduct3", "InventoryLocation3");
            var time = new DateTime(2025, 1, 1, 10, 0, 0);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => productLocation.AddInventory(time, -10));
        }

        [Fact]
        public void AddInventory_AtSameTime_ShouldAccumulate()
        {
            // Arrange
            var productLocation = ProductLocation.Create("InventoryProduct4", "InventoryLocation4");
            var time = new DateTime(2025, 1, 1, 10, 0, 0);

            // Act
            productLocation.AddInventory(time, 50.0);
            productLocation.AddInventory(time, 30.0);

            // Assert
            var change = productLocation.GetInventoryChangeAtTime(time);
            Assert.Equal(80.0, change);
        }

        [Fact]
        public void RemoveInventory_WithValidQuantity_ShouldSubtractFromInventoryProfile()
        {
            // Arrange
            var productLocation = ProductLocation.Create("InventoryProduct5", "InventoryLocation5");
            var time = new DateTime(2025, 1, 1, 10, 0, 0);
            var quantity = 50.0;

            // Act
            productLocation.RemoveInventory(time, quantity);

            // Assert
            var change = productLocation.GetInventoryChangeAtTime(time);
            Assert.Equal(-quantity, change);
        }

        [Fact]
        public void RemoveInventory_WithZeroQuantity_ShouldThrowArgumentException()
        {
            // Arrange
            var productLocation = ProductLocation.Create("InventoryProduct6", "InventoryLocation6");
            var time = new DateTime(2025, 1, 1, 10, 0, 0);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => productLocation.RemoveInventory(time, 0));
        }

        [Fact]
        public void RemoveInventory_WithNegativeQuantity_ShouldThrowArgumentException()
        {
            // Arrange
            var productLocation = ProductLocation.Create("InventoryProduct7", "InventoryLocation7");
            var time = new DateTime(2025, 1, 1, 10, 0, 0);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => productLocation.RemoveInventory(time, -10));
        }

        [Fact]
        public void UpdateInventory_ShouldSetNetChange()
        {
            // Arrange
            var productLocation = ProductLocation.Create("InventoryProduct8", "InventoryLocation8");
            var time = new DateTime(2025, 1, 1, 10, 0, 0);

            // Act
            productLocation.UpdateInventory(time, 75.0);

            // Assert
            var change = productLocation.GetInventoryChangeAtTime(time);
            Assert.Equal(75.0, change);
        }

        [Fact]
        public void UpdateInventory_ShouldOverwriteExistingChange()
        {
            // Arrange
            var productLocation = ProductLocation.Create("InventoryProduct9", "InventoryLocation9");
            var time = new DateTime(2025, 1, 1, 10, 0, 0);
            productLocation.AddInventory(time, 100.0);

            // Act
            productLocation.UpdateInventory(time, 50.0);

            // Assert
            var change = productLocation.GetInventoryChangeAtTime(time);
            Assert.Equal(50.0, change);
        }

        [Fact]
        public void GetCumulativeInventory_ShouldSumAllChangesUpToTime()
        {
            // Arrange
            var productLocation = ProductLocation.Create("InventoryProduct10", "InventoryLocation10");
            var time1 = new DateTime(2025, 1, 1, 10, 0, 0);
            var time2 = new DateTime(2025, 1, 1, 12, 0, 0);
            var time3 = new DateTime(2025, 1, 1, 14, 0, 0);

            // Act
            productLocation.AddInventory(time1, 100.0);
            productLocation.AddInventory(time2, 50.0);
            productLocation.RemoveInventory(time3, 30.0);

            var cumulativeAtTime2 = productLocation.GetCumulativeInventory(time2);
            var cumulativeAtTime3 = productLocation.GetCumulativeInventory(time3);

            // Assert
            Assert.Equal(150.0, cumulativeAtTime2);
            Assert.Equal(120.0, cumulativeAtTime3);
        }

        [Fact]
        public void GetCumulativeInventory_WithFutureTime_ShouldNotIncludeFutureChanges()
        {
            // Arrange
            var productLocation = ProductLocation.Create("InventoryProduct11", "InventoryLocation11");
            var time1 = new DateTime(2025, 1, 1, 10, 0, 0);
            var time2 = new DateTime(2025, 1, 1, 14, 0, 0);
            var queryTime = new DateTime(2025, 1, 1, 12, 0, 0);

            // Act
            productLocation.AddInventory(time1, 100.0);
            productLocation.AddInventory(time2, 50.0);

            var cumulative = productLocation.GetCumulativeInventory(queryTime);

            // Assert
            Assert.Equal(100.0, cumulative);
        }

        [Fact]
        public void GetInventoryChangeAtTime_WithNoChange_ShouldReturnZero()
        {
            // Arrange
            var productLocation = ProductLocation.Create("InventoryProduct12", "InventoryLocation12");
            var time = new DateTime(2025, 1, 1, 10, 0, 0);

            // Act
            var change = productLocation.GetInventoryChangeAtTime(time);

            // Assert
            Assert.Equal(0.0, change);
        }

        [Fact]
        public void GetAllInventoryChanges_ShouldReturnAllChangesInChronologicalOrder()
        {
            // Arrange
            var productLocation = ProductLocation.Create("InventoryProduct13", "InventoryLocation13");
            var time1 = new DateTime(2025, 1, 1, 12, 0, 0);
            var time2 = new DateTime(2025, 1, 1, 8, 0, 0);
            var time3 = new DateTime(2025, 1, 1, 16, 0, 0);

            // Act
            productLocation.AddInventory(time1, 100.0);
            productLocation.AddInventory(time2, 50.0);
            productLocation.RemoveInventory(time3, 25.0);

            var changes = productLocation.GetAllInventoryChanges();

            // Assert
            Assert.Equal(3, changes.Count);
            Assert.Equal(time2, changes[0].Time);
            Assert.Equal(50.0, changes[0].NetChange);
            Assert.Equal(time1, changes[1].Time);
            Assert.Equal(100.0, changes[1].NetChange);
            Assert.Equal(time3, changes[2].Time);
            Assert.Equal(-25.0, changes[2].NetChange);
        }

        [Fact]
        public void GetAllInventoryChanges_WithNoChanges_ShouldReturnEmptyList()
        {
            // Arrange
            var productLocation = ProductLocation.Create("InventoryProduct14", "InventoryLocation14");

            // Act
            var changes = productLocation.GetAllInventoryChanges();

            // Assert
            Assert.Empty(changes);
        }

        [Fact]
        public void GetInventoryChangesInRange_ShouldReturnOnlyChangesInRange()
        {
            // Arrange
            var productLocation = ProductLocation.Create("InventoryProduct15", "InventoryLocation15");
            var time1 = new DateTime(2025, 1, 1, 8, 0, 0);
            var time2 = new DateTime(2025, 1, 1, 12, 0, 0);
            var time3 = new DateTime(2025, 1, 1, 16, 0, 0);
            var time4 = new DateTime(2025, 1, 1, 20, 0, 0);

            productLocation.AddInventory(time1, 50.0);
            productLocation.AddInventory(time2, 100.0);
            productLocation.AddInventory(time3, 75.0);
            productLocation.AddInventory(time4, 25.0);

            var startTime = new DateTime(2025, 1, 1, 10, 0, 0);
            var endTime = new DateTime(2025, 1, 1, 18, 0, 0);

            // Act
            var changes = productLocation.GetInventoryChangesInRange(startTime, endTime);

            // Assert
            Assert.Equal(2, changes.Count);
            Assert.Equal(time2, changes[0].Time);
            Assert.Equal(100.0, changes[0].NetChange);
            Assert.Equal(time3, changes[1].Time);
            Assert.Equal(75.0, changes[1].NetChange);
        }

        [Fact]
        public void GetInventoryChangesInRange_WithNoChangesInRange_ShouldReturnEmptyList()
        {
            // Arrange
            var productLocation = ProductLocation.Create("InventoryProduct16", "InventoryLocation16");
            var time1 = new DateTime(2025, 1, 1, 8, 0, 0);
            productLocation.AddInventory(time1, 50.0);

            var startTime = new DateTime(2025, 1, 1, 10, 0, 0);
            var endTime = new DateTime(2025, 1, 1, 18, 0, 0);

            // Act
            var changes = productLocation.GetInventoryChangesInRange(startTime, endTime);

            // Assert
            Assert.Empty(changes);
        }

        [Fact]
        public void InventoryProfile_ShouldBeAccessible()
        {
            // Arrange
            var productLocation = ProductLocation.Create("InventoryProduct17", "InventoryLocation17");

            // Act & Assert
            Assert.NotNull(productLocation.InventoryProfile);
        }

        [Fact]
        public void ComplexInventoryScenario_ShouldMaintainCorrectState()
        {
            // Arrange
            var productLocation = ProductLocation.Create("InventoryProduct18", "InventoryLocation18");
            var time1 = new DateTime(2025, 1, 1, 8, 0, 0);
            var time2 = new DateTime(2025, 1, 1, 10, 0, 0);
            var time3 = new DateTime(2025, 1, 1, 12, 0, 0);
            var time4 = new DateTime(2025, 1, 1, 14, 0, 0);
            var time5 = new DateTime(2025, 1, 1, 16, 0, 0);

            // Act - Complex series of operations
            productLocation.AddInventory(time1, 100.0);
            productLocation.AddInventory(time2, 50.0);
            productLocation.RemoveInventory(time3, 30.0);
            productLocation.AddInventory(time4, 25.0);
            productLocation.UpdateInventory(time5, 10.0);

            // Assert
            Assert.Equal(100.0, productLocation.GetInventoryChangeAtTime(time1));
            Assert.Equal(50.0, productLocation.GetInventoryChangeAtTime(time2));
            Assert.Equal(-30.0, productLocation.GetInventoryChangeAtTime(time3));
            Assert.Equal(25.0, productLocation.GetInventoryChangeAtTime(time4));
            Assert.Equal(10.0, productLocation.GetInventoryChangeAtTime(time5));

            var cumulative1 = productLocation.GetCumulativeInventory(time1);
            var cumulative3 = productLocation.GetCumulativeInventory(time3);
            var cumulative5 = productLocation.GetCumulativeInventory(time5);

            Assert.Equal(100.0, cumulative1);
            Assert.Equal(120.0, cumulative3);
            Assert.Equal(155.0, cumulative5);

            var allChanges = productLocation.GetAllInventoryChanges();
            Assert.Equal(5, allChanges.Count);
        }
    }
}

