using System.Linq;
using Scheduling.Models;
using Xunit;

namespace Scheduling.Tests
{
    [Collection("Serial")]
    public class AlternateOperationTests : TestBase
    {

        [Fact]
        public void ProcessAlternateOperations_WithMultipleOperationsProducingSameProductLocation_ShouldCreateAlternateOperation()
        {
            // Arrange: Create ProductLocation
            var carAtBangalore = ProductLocation.Create("Car", "Bangalore");

            // Create first operation
            var tyreFlow1 = new Flow(ProductLocation.Create("Tyre", "Bangalore"), 4.0);
            var engineFlow1 = new Flow(ProductLocation.Create("Engine", "Bangalore"), 1.0);
            var consumeFlows1 = new SimConsumeFlow(new[] { tyreFlow1, engineFlow1 });
            var carFlow1 = new Flow(carAtBangalore, 1.0);
            var produceFlow1 = new ProduceFlow(carFlow1);
            var operation1 = new Operation("MakeCarInBangalore1", priority: 1, consumeFlow: consumeFlows1, produceFlow: produceFlow1);

            // Create second operation producing the same ProductLocation
            var tyreFlow2 = new Flow(ProductLocation.Create("Tyre", "Bangalore"), 3.0);
            var engineFlow2 = new Flow(ProductLocation.Create("Engine", "Bangalore"), 1.0);
            var consumeFlows2 = new SimConsumeFlow(new[] { tyreFlow2, engineFlow2 });
            var carFlow2 = new Flow(carAtBangalore, 1.0);
            var produceFlow2 = new ProduceFlow(carFlow2);
            var operation2 = new Operation("MakeCarInBangalore2", priority: 2, consumeFlow: consumeFlows2, produceFlow: produceFlow2);

            // Act: Process alternate operations
            Operation.ProcessAlternateOperations();

            // Assert: Verify AlternateOperation was created
            Assert.NotNull(carAtBangalore.ProducingOperation);
            Assert.Equal(OperationType.Alternate, carAtBangalore.ProducingOperation!.GetOperationType());

            var alternateOp = carAtBangalore.ProducingOperation as AlternateOperation;
            Assert.NotNull(alternateOp);
            Assert.Equal(2, alternateOp!.Count);

            // Verify operations are included
            var operations = alternateOp.Operations.ToList();
            Assert.Contains(operations, op => op.Operation.Key == "MakeCarInBangalore1" && op.Priority == 1);
            Assert.Contains(operations, op => op.Operation.Key == "MakeCarInBangalore2" && op.Priority == 2);
        }

        [Fact]
        public void ProcessAlternateOperations_WithSingleOperationProducingProductLocation_ShouldSetOperationDirectly()
        {
            // Arrange: Create ProductLocation and single operation
            var carAtBangalore = ProductLocation.Create("Car", "Bangalore");
            var tyreFlow = new Flow(ProductLocation.Create("Tyre", "Bangalore"), 4.0);
            var engineFlow = new Flow(ProductLocation.Create("Engine", "Bangalore"), 1.0);
            var consumeFlows = new SimConsumeFlow(new[] { tyreFlow, engineFlow });
            var carFlow = new Flow(carAtBangalore, 1.0);
            var produceFlow = new ProduceFlow(carFlow);
            var operation = new Operation("MakeCarInBangalore3", priority: 1, consumeFlow: consumeFlows, produceFlow: produceFlow);

            // Act: Process alternate operations
            Operation.ProcessAlternateOperations();

            // Assert: Verify Operation was set directly (not AlternateOperation)
            Assert.NotNull(carAtBangalore.ProducingOperation);
            Assert.Equal(OperationType.Basic, carAtBangalore.ProducingOperation!.GetOperationType());
            Assert.Equal(operation, carAtBangalore.ProducingOperation);
        }

        [Fact]
        public void ProcessAlternateOperations_WithOperationsProducingDifferentProductLocations_ShouldSetEachIndependently()
        {
            // Arrange: Create two ProductLocations with different operations
            var carAtBangalore = ProductLocation.Create("Car", "Bangalore");
            var carAtMumbai = ProductLocation.Create("Car", "Mumbai");

            // Operation for Bangalore
            var tyreFlow1 = new Flow(ProductLocation.Create("Tyre", "Bangalore"), 4.0);
            var consumeFlows1 = new SimConsumeFlow(new[] { tyreFlow1 });
            var carFlow1 = new Flow(carAtBangalore, 1.0);
            var produceFlow1 = new ProduceFlow(carFlow1);
            var operation1 = new Operation("MakeCarInBangalore", priority: 1, consumeFlow: consumeFlows1, produceFlow: produceFlow1);

            // Operation for Mumbai
            var tyreFlow2 = new Flow(ProductLocation.Create("Tyre", "Mumbai"), 4.0);
            var consumeFlows2 = new SimConsumeFlow(new[] { tyreFlow2 });
            var carFlow2 = new Flow(carAtMumbai, 1.0);
            var produceFlow2 = new ProduceFlow(carFlow2);
            var operation2 = new Operation("MakeCarInMumbai", priority: 1, consumeFlow: consumeFlows2, produceFlow: produceFlow2);

            // Act: Process alternate operations
            Operation.ProcessAlternateOperations();

            // Assert: Each ProductLocation should have its own operation
            Assert.NotNull(carAtBangalore.ProducingOperation);
            Assert.Equal(OperationType.Basic, carAtBangalore.ProducingOperation!.GetOperationType());
            Assert.Equal(operation1, carAtBangalore.ProducingOperation);

            Assert.NotNull(carAtMumbai.ProducingOperation);
            Assert.Equal(OperationType.Basic, carAtMumbai.ProducingOperation!.GetOperationType());
            Assert.Equal(operation2, carAtMumbai.ProducingOperation);
        }

        [Fact]
        public void AlternateOperation_GetOperationsByPriority_ShouldReturnSortedByPriority()
        {
            // Arrange
            var carAtBangalore = ProductLocation.Create("Car", "Bangalore");
            var carFlow = new Flow(carAtBangalore, 1.0);
            var produceFlow = new ProduceFlow(carFlow);

            var operation1 = new Operation("Op1", priority: 3, produceFlow: produceFlow);
            var operation2 = new Operation("Op2", priority: 1, produceFlow: produceFlow);
            var operation3 = new Operation("Op3", priority: 2, produceFlow: produceFlow);

            var operations = new[]
            {
                new OperationWithPriority(operation1, operation1.Priority),
                new OperationWithPriority(operation2, operation2.Priority),
                new OperationWithPriority(operation3, operation3.Priority)
            };

            var alternateOp = new AlternateOperation(operations);

            // Act
            var sortedOps = alternateOp.GetOperationsByPriority().ToList();

            // Assert: Should be sorted by priority (1, 2, 3)
            Assert.Equal(3, sortedOps.Count);
            Assert.Equal(1, sortedOps[0].Priority);
            Assert.Equal("Op2", sortedOps[0].Operation.Key);
            Assert.Equal(2, sortedOps[1].Priority);
            Assert.Equal("Op3", sortedOps[1].Operation.Key);
            Assert.Equal(3, sortedOps[2].Priority);
            Assert.Equal("Op1", sortedOps[2].Operation.Key);
        }
    }
}

