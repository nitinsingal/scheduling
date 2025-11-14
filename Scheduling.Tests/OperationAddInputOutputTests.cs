using System;
using System.Linq;
using Scheduling.Models;
using Xunit;

namespace Scheduling.Tests
{
    [Collection("Serial")]
    public class OperationAddInputOutputTests : TestBase
    {

        [Fact]
        public void AddInput_WithNoExistingConsumeFlow_ShouldCreateSimConsumeFlow()
        {
            // Arrange
            var operation = new Operation("TestOp", priority: 1);
            var tyreAtBangalore = ProductLocation.Create("Tyre", "Bangalore");

            // Act
            operation.AddInput(tyreAtBangalore, 4.0);

            // Assert
            Assert.True(operation.HasConsumption);
            Assert.True(operation.IsConsumeFlowSimFlow);
            var flows = operation.GetConsumeFlows().ToList();
            Assert.Single(flows);
            Assert.Equal(tyreAtBangalore, flows[0].ProductLocation);
            Assert.Equal(4.0, flows[0].QuantityPer);
        }

        [Fact]
        public void AddInput_WithExistingConsumeFlow_ShouldConvertToSimConsumeFlow()
        {
            // Arrange
            var tyreAtBangalore = ProductLocation.Create("Tyre", "Bangalore");
            var engineAtBangalore = ProductLocation.Create("Engine", "Bangalore");
            var tyreFlow = new Flow(tyreAtBangalore, 4.0);
            var consumeFlow = new ConsumeFlow(tyreFlow);
            var operation = new Operation("TestOp", priority: 1, consumeFlow: consumeFlow);

            // Act
            operation.AddInput(engineAtBangalore, 1.0);

            // Assert
            Assert.True(operation.HasConsumption);
            Assert.True(operation.IsConsumeFlowSimFlow);
            var flows = operation.GetConsumeFlows().ToList();
            Assert.Equal(2, flows.Count);
            Assert.Contains(flows, f => f.ProductLocation == tyreAtBangalore && f.QuantityPer == 4.0);
            Assert.Contains(flows, f => f.ProductLocation == engineAtBangalore && f.QuantityPer == 1.0);
        }

        [Fact]
        public void AddInput_WithExistingSimConsumeFlow_ShouldAddToSimConsumeFlow()
        {
            // Arrange
            var tyreAtBangalore = ProductLocation.Create("Tyre", "Bangalore");
            var engineAtBangalore = ProductLocation.Create("Engine", "Bangalore");
            var tyreFlow = new Flow(tyreAtBangalore, 4.0);
            var engineFlow = new Flow(engineAtBangalore, 1.0);
            var simConsumeFlow = new SimConsumeFlow(new[] { tyreFlow });
            var operation = new Operation("TestOp", priority: 1, consumeFlow: simConsumeFlow);

            // Act
            operation.AddInput(engineAtBangalore, 1.0);

            // Assert
            var flows = operation.GetConsumeFlows().ToList();
            Assert.Equal(2, flows.Count);
            Assert.Contains(flows, f => f.ProductLocation == tyreAtBangalore);
            Assert.Contains(flows, f => f.ProductLocation == engineAtBangalore && f.QuantityPer == 1.0);
        }

        [Fact]
        public void AddInput_WithDuplicateProductLocation_ShouldLogWarningAndReturn()
        {
            // Arrange
            var tyreAtBangalore = ProductLocation.Create("Tyre", "Bangalore");
            var operation = new Operation("TestOp", priority: 1);
            operation.AddInput(tyreAtBangalore, 4.0);

            // Act & Assert - should not throw, just log warning
            operation.AddInput(tyreAtBangalore, 5.0);

            // Verify only one flow exists (original, not updated)
            var flows = operation.GetConsumeFlows().ToList();
            Assert.Single(flows);
            Assert.Equal(4.0, flows[0].QuantityPer); // Original quantity, not updated
        }

        [Fact]
        public void AddOutput_WithNoExistingProduceFlow_ShouldCreateProduceFlow()
        {
            // Arrange
            var operation = new Operation("TestOp", priority: 1);
            var carAtBangalore = ProductLocation.Create("Car", "Bangalore");

            // Act
            operation.AddOutput(carAtBangalore, 1.0);

            // Assert
            Assert.True(operation.HasProduction);
            var produceFlow = operation.GetProduceFlow();
            Assert.NotNull(produceFlow);
            Assert.Equal(carAtBangalore, produceFlow!.ProductLocation);
            Assert.Equal(1.0, produceFlow.QuantityPer);
        }

        [Fact]
        public void AddOutput_WithSameProductLocation_ShouldLogWarningAndReturn()
        {
            // Arrange
            var carAtBangalore = ProductLocation.Create("Car", "Bangalore");
            var operation = new Operation("TestOp", priority: 1);
            operation.AddOutput(carAtBangalore, 1.0);

            // Act & Assert - should not throw, just log warning
            operation.AddOutput(carAtBangalore, 2.0);

            // Verify original quantity is preserved
            var produceFlow = operation.GetProduceFlow();
            Assert.NotNull(produceFlow);
            Assert.Equal(1.0, produceFlow!.QuantityPer); // Original quantity, not updated
        }

        [Fact]
        public void AddOutput_WithDifferentProductLocation_ShouldThrowException()
        {
            // Arrange
            var carAtBangalore = ProductLocation.Create("Car", "Bangalore");
            var carAtMumbai = ProductLocation.Create("Car", "Mumbai");
            var operation = new Operation("TestOp", priority: 1);
            operation.AddOutput(carAtBangalore, 1.0);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                operation.AddOutput(carAtMumbai, 1.0));

            Assert.Contains("Multiple outputs are not supported yet", exception.Message);
            Assert.Contains("ProduceFlow", exception.Message);
            Assert.Contains("Car@Bangalore", exception.Message);
            Assert.Contains("Car@Mumbai", exception.Message);
        }

        [Fact]
        public void AddInput_WithInvalidQuantity_ShouldThrowException()
        {
            // Arrange
            var operation = new Operation("TestOp", priority: 1);
            var tyreAtBangalore = ProductLocation.Create("Tyre", "Bangalore");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => operation.AddInput(tyreAtBangalore, 0));
            Assert.Throws<ArgumentException>(() => operation.AddInput(tyreAtBangalore, -1));
        }

        [Fact]
        public void AddOutput_WithInvalidQuantity_ShouldThrowException()
        {
            // Arrange
            var operation = new Operation("TestOp", priority: 1);
            var carAtBangalore = ProductLocation.Create("Car", "Bangalore");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => operation.AddOutput(carAtBangalore, 0));
            Assert.Throws<ArgumentException>(() => operation.AddOutput(carAtBangalore, -1));
        }

        [Fact]
        public void AddInput_WithNullProductLocation_ShouldThrowException()
        {
            // Arrange
            var operation = new Operation("TestOp", priority: 1);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => operation.AddInput(null!, 1.0));
        }

        [Fact]
        public void AddOutput_WithNullProductLocation_ShouldThrowException()
        {
            // Arrange
            var operation = new Operation("TestOp", priority: 1);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => operation.AddOutput(null!, 1.0));
        }

        [Fact]
        public void AddInput_AddMultipleInputs_ShouldCreateSimConsumeFlowWithAll()
        {
            // Arrange
            var operation = new Operation("TestOp", priority: 1);
            var tyreAtBangalore = ProductLocation.Create("Tyre", "Bangalore");
            var engineAtBangalore = ProductLocation.Create("Engine", "Bangalore");
            var wheelAtBangalore = ProductLocation.Create("Wheel", "Bangalore");

            // Act
            operation.AddInput(tyreAtBangalore, 4.0);
            operation.AddInput(engineAtBangalore, 1.0);
            operation.AddInput(wheelAtBangalore, 4.0);

            // Assert
            var flows = operation.GetConsumeFlows().ToList();
            Assert.Equal(3, flows.Count);
            Assert.Contains(flows, f => f.ProductLocation == tyreAtBangalore && f.QuantityPer == 4.0);
            Assert.Contains(flows, f => f.ProductLocation == engineAtBangalore && f.QuantityPer == 1.0);
            Assert.Contains(flows, f => f.ProductLocation == wheelAtBangalore && f.QuantityPer == 4.0);
        }
    }
}

