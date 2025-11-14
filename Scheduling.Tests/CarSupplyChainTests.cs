using System.Linq;
using Scheduling.Models;
using Xunit;

namespace Scheduling.Tests
{
    [Collection("Serial")]
    public class CarSupplyChainTests : TestBase
    {

        [Fact]
        public void CreateCarSupplyChain_ShouldCreateCompleteSupplyChain()
        {
            // Arrange & Act: Create Products
            var carProduct = Product.Create("Car");
            var tyreProduct = Product.Create("Tyre");
            var engineProduct = Product.Create("Engine");

            // Create Location
            var bangaloreLocation = Location.Create("Bangalore");

            // Create ProductLocations
            var carAtBangalore = ProductLocation.Create("Car", "Bangalore");
            var tyreAtBangalore = ProductLocation.Create("Tyre", "Bangalore");
            var engineAtBangalore = ProductLocation.Create("Engine", "Bangalore");

            // Create Operation: MakeCarInBangalore
            var makeCarInBangalore = new Operation("MakeCarInBangalore", priority: 1);

            // Add inputs using AddInput method
            // 4 Tyres per Car
            makeCarInBangalore.AddInput(tyreAtBangalore, 4.0);
            // 1 Engine per Car
            makeCarInBangalore.AddInput(engineAtBangalore, 1.0);

            // Add output using AddOutput method
            // 1 Car per operation
            makeCarInBangalore.AddOutput(carAtBangalore, 1.0);

            // Process alternate operations to set producing operation
            Operation.ProcessAlternateOperations();

            // Assert: Verify Products
            Assert.NotNull(carProduct);
            Assert.Equal("Car", carProduct.Name);
            Assert.NotNull(tyreProduct);
            Assert.Equal("Tyre", tyreProduct.Name);
            Assert.NotNull(engineProduct);
            Assert.Equal("Engine", engineProduct.Name);


            // Assert: Verify Location
            Assert.NotNull(bangaloreLocation);
            Assert.Equal("Bangalore", bangaloreLocation.Name);

            // Assert: Verify ProductLocations
            Assert.NotNull(carAtBangalore);
            Assert.Equal("Car@Bangalore", carAtBangalore.Key);
            Assert.NotNull(tyreAtBangalore);
            Assert.Equal("Tyre@Bangalore", tyreAtBangalore.Key);
            Assert.NotNull(engineAtBangalore);
            Assert.Equal("Engine@Bangalore", engineAtBangalore.Key);

            // Assert: Verify consume flows from operation
            var consumeFlowsList = makeCarInBangalore.GetConsumeFlows().ToList();
            Assert.Equal(2, consumeFlowsList.Count);

            var tyreFlow = consumeFlowsList.First(f => f.ProductLocation == tyreAtBangalore);
            Assert.NotNull(tyreFlow);
            Assert.Equal(4.0, tyreFlow.QuantityPer);
            Assert.Equal(tyreAtBangalore, tyreFlow.ProductLocation);

            var engineFlow = consumeFlowsList.First(f => f.ProductLocation == engineAtBangalore);
            Assert.NotNull(engineFlow);
            Assert.Equal(1.0, engineFlow.QuantityPer);
            Assert.Equal(engineAtBangalore, engineFlow.ProductLocation);

            // Assert: Verify produce flow from operation
            var carFlow = makeCarInBangalore.GetProduceFlow();
            Assert.NotNull(carFlow);
            Assert.Equal(1.0, carFlow.QuantityPer);
            Assert.Equal(carAtBangalore, carFlow.ProductLocation);

            // Assert: Verify Operation
            Assert.NotNull(makeCarInBangalore);
            Assert.True(makeCarInBangalore.HasConsumption);
            Assert.True(makeCarInBangalore.HasProduction);
            Assert.True(makeCarInBangalore.IsConsumeFlowSimFlow);
            Assert.False(makeCarInBangalore.IsConsumeFlowSingle);
            Assert.Equal(OperationType.Basic, makeCarInBangalore.GetOperationType());

            // Assert: Verify ProducingOperation on Car@Bangalore (set by ProcessAlternateOperations)
            Assert.NotNull(carAtBangalore.ProducingOperation);
            Assert.Equal(makeCarInBangalore, carAtBangalore.ProducingOperation);
            Assert.Equal(OperationType.Basic, carAtBangalore.ProducingOperation!.GetOperationType());
            Operation.Clear(); // Clear operations to avoid conflicts in next test
        }

        [Fact]
        public void CreateCarSupplyChain_VerifyOperationConsumesCorrectQuantities()
        {
            // Arrange: Create the supply chain
            var carAtBangalore = ProductLocation.Create("Car", "Bangalore");
            var tyreAtBangalore = ProductLocation.Create("Tyre", "Bangalore");
            var engineAtBangalore = ProductLocation.Create("Engine", "Bangalore");

            // Create Operation and add inputs/outputs using AddInput/AddOutput
            var makeCarInBangalore = new Operation("MakeCarInBangalore", priority: 1);
            makeCarInBangalore.AddInput(tyreAtBangalore, 4.0);
            makeCarInBangalore.AddInput(engineAtBangalore, 1.0);
            makeCarInBangalore.AddOutput(carAtBangalore, 1.0);

            // Act: Get consume flows from operation
            var consumedFlows = makeCarInBangalore.GetConsumeFlows().ToList();

            // Assert: Verify quantities
            var tyreConsumeFlow = consumedFlows.First(f => f.ProductLocation == tyreAtBangalore);
            Assert.Equal(4.0, tyreConsumeFlow.QuantityPer);

            var engineConsumeFlow = consumedFlows.First(f => f.ProductLocation == engineAtBangalore);
            Assert.Equal(1.0, engineConsumeFlow.QuantityPer);
        }

        [Fact]
        public void CreateCarSupplyChain_VerifyProducingOperationIsSet()
        {
            // Arrange
            var carAtBangalore = ProductLocation.Create("Car", "Bangalore");
            var tyreAtBangalore = ProductLocation.Create("Tyre", "Bangalore");
            var engineAtBangalore = ProductLocation.Create("Engine", "Bangalore");

            // Create Operation and add inputs/outputs using AddInput/AddOutput
            var makeCarInBangalore = new Operation("MakeCarInBangalore", priority: 1);
            makeCarInBangalore.AddInput(tyreAtBangalore, 4.0);
            makeCarInBangalore.AddInput(engineAtBangalore, 1.0);
            makeCarInBangalore.AddOutput(carAtBangalore, 1.0);

            // Act: Process alternate operations to set producing operation
            Operation.ProcessAlternateOperations();

            // Assert
            Assert.NotNull(carAtBangalore.ProducingOperation);
            var producingOp = carAtBangalore.ProducingOperation;
            Assert.Equal(OperationType.Basic, producingOp!.GetOperationType());
            Assert.Equal(makeCarInBangalore, producingOp);
        }
    }
}

