using System.Linq;
using Scheduling.Models;
using Xunit;

namespace Scheduling.Tests
{
    [Collection("Serial")]
    public class ProductTests : TestBase
    {
        [Fact]
        public void Create_WithValidName_ShouldCreateProduct()
        {
            // Arrange
            var productName = "TestProduct";

            // Act
            var product = Product.Create(productName);

            // Assert
            Assert.NotNull(product);
            Assert.Equal(productName, product.Name);
        }

        [Fact]
        public void Create_WithSameName_ShouldReturnSameInstance()
        {
            // Arrange
            var productName = "TestProduct";

            // Act
            var product1 = Product.Create(productName);
            var product2 = Product.Create(productName);

            // Assert
            Assert.Same(product1, product2);
        }

        [Fact]
        public void Create_WithNullName_ShouldThrowArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Product.Create(null!));
        }

        [Fact]
        public void Create_WithEmptyName_ShouldThrowArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Product.Create(""));
        }

        [Fact]
        public void Create_WithWhitespaceName_ShouldThrowArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Product.Create("   "));
        }

        [Fact]
        public void Get_WithExistingProduct_ShouldReturnProduct()
        {
            // Arrange
            var productName = "ExistingProduct";
            Product.Create(productName);

            // Act
            var product = Product.Get(productName);

            // Assert
            Assert.NotNull(product);
            Assert.Equal(productName, product!.Name);
        }

        [Fact]
        public void Get_WithNonExistingProduct_ShouldReturnNull()
        {
            // Act
            var product = Product.Get("NonExistingProduct");

            // Assert
            Assert.Null(product);
        }

        [Fact]
        public void Exists_WithExistingProduct_ShouldReturnTrue()
        {
            // Arrange
            var productName = "ExistingProduct";
            Product.Create(productName);

            // Act
            var exists = Product.Exists(productName);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public void Exists_WithNonExistingProduct_ShouldReturnFalse()
        {
            // Act
            var exists = Product.Exists("NonExistingProduct");

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public void Remove_WithExistingProduct_ShouldReturnTrue()
        {
            // Arrange
            var productName = "ProductToRemove";
            Product.Create(productName);

            // Act
            var removed = Product.Remove(productName);

            // Assert
            Assert.True(removed);
            Assert.False(Product.Exists(productName));
        }

        [Fact]
        public void Remove_WithNonExistingProduct_ShouldReturnFalse()
        {
            // Act
            var removed = Product.Remove("NonExistingProduct");

            // Assert
            Assert.False(removed);
        }

        [Fact]
        public void GetAll_ShouldReturnAllProducts()
        {
            // Arrange
            Product.Create("Product1");
            Product.Create("Product2");
            Product.Create("Product3");

            // Act
            var products = Product.GetAll().ToList();

            // Assert
            Assert.True(products.Count >= 3);
            Assert.Contains(products, p => p.Name == "Product1");
            Assert.Contains(products, p => p.Name == "Product2");
            Assert.Contains(products, p => p.Name == "Product3");
        }
    }
}

