using System;
using System.Collections.Generic;
using System.Linq;

namespace Scheduling.Models
{
    /// <summary>
    /// Tracks inventory changes over time for a product at a location.
    /// Uses a time-ordered structure to record additions and removals.
    /// </summary>
    public class InventoryProfile
    {
        private readonly SortedDictionary<DateTime, double> _inventoryChanges;

        public InventoryProfile()
        {
            _inventoryChanges = new SortedDictionary<DateTime, double>();
        }

        /// <summary>
        /// Adds inventory at a specific time.
        /// </summary>
        /// <param name="time">The time when inventory is added</param>
        /// <param name="quantity">The quantity to add (positive value)</param>
        public void AddInventory(DateTime time, double quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be positive when adding inventory.", nameof(quantity));
            }

            if (_inventoryChanges.ContainsKey(time))
            {
                _inventoryChanges[time] += quantity;
            }
            else
            {
                _inventoryChanges[time] = quantity;
            }
        }

        /// <summary>
        /// Removes inventory at a specific time.
        /// </summary>
        /// <param name="time">The time when inventory is removed</param>
        /// <param name="quantity">The quantity to remove (positive value, stored as negative)</param>
        public void RemoveInventory(DateTime time, double quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be positive when removing inventory.", nameof(quantity));
            }

            if (_inventoryChanges.ContainsKey(time))
            {
                _inventoryChanges[time] -= quantity;
            }
            else
            {
                _inventoryChanges[time] = -quantity;
            }
        }

        /// <summary>
        /// Updates inventory at a specific time to a specific net change.
        /// </summary>
        /// <param name="time">The time of the inventory change</param>
        /// <param name="netChange">The net change in inventory (positive for addition, negative for removal)</param>
        public void UpdateInventory(DateTime time, double netChange)
        {
            if (_inventoryChanges.ContainsKey(time))
            {
                _inventoryChanges[time] = netChange;
            }
            else
            {
                _inventoryChanges[time] = netChange;
            }
        }

        /// <summary>
        /// Gets the net inventory change at a specific time.
        /// </summary>
        /// <param name="time">The time to query</param>
        /// <returns>The net change at that time, or 0 if no change recorded</returns>
        public double GetInventoryChangeAtTime(DateTime time)
        {
            return _inventoryChanges.TryGetValue(time, out var quantity) ? quantity : 0.0;
        }

        /// <summary>
        /// Gets the cumulative inventory level at a specific time (sum of all changes up to that time).
        /// </summary>
        /// <param name="time">The time to query</param>
        /// <returns>The cumulative inventory level</returns>
        public double GetCumulativeInventory(DateTime time)
        {
            double cumulative = 0;
            foreach (var kvp in _inventoryChanges)
            {
                if (kvp.Key <= time)
                {
                    cumulative += kvp.Value;
                }
                else
                {
                    break;
                }
            }
            return cumulative;
        }

        /// <summary>
        /// Gets all inventory changes in chronological order.
        /// </summary>
        /// <returns>List of tuples containing (time, netChange)</returns>
        public List<(DateTime Time, double NetChange)> GetAllChanges()
        {
            return _inventoryChanges.Select(kvp => (kvp.Key, kvp.Value)).ToList();
        }

        /// <summary>
        /// Gets all inventory changes within a time range.
        /// </summary>
        /// <param name="startTime">Start of the time range (inclusive)</param>
        /// <param name="endTime">End of the time range (inclusive)</param>
        /// <returns>List of tuples containing (time, netChange)</returns>
        public List<(DateTime Time, double NetChange)> GetChangesInRange(DateTime startTime, DateTime endTime)
        {
            return _inventoryChanges
                .Where(kvp => kvp.Key >= startTime && kvp.Key <= endTime)
                .Select(kvp => (kvp.Key, kvp.Value))
                .ToList();
        }

        /// <summary>
        /// Removes an inventory change at a specific time.
        /// </summary>
        /// <param name="time">The time of the change to remove</param>
        /// <returns>True if a change was removed, false if no change existed at that time</returns>
        public bool RemoveInventoryChange(DateTime time)
        {
            return _inventoryChanges.Remove(time);
        }

        /// <summary>
        /// Clears all inventory changes.
        /// </summary>
        public void Clear()
        {
            _inventoryChanges.Clear();
        }

        /// <summary>
        /// Gets the count of inventory change events.
        /// </summary>
        public int ChangeCount => _inventoryChanges.Count;

        /// <summary>
        /// Gets the earliest time with an inventory change.
        /// </summary>
        /// <returns>The earliest time, or null if no changes exist</returns>
        public DateTime? GetEarliestChangeTime()
        {
            return _inventoryChanges.Count > 0 ? _inventoryChanges.Keys.First() : null;
        }

        /// <summary>
        /// Gets the latest time with an inventory change.
        /// </summary>
        /// <returns>The latest time, or null if no changes exist</returns>
        public DateTime? GetLatestChangeTime()
        {
            return _inventoryChanges.Count > 0 ? _inventoryChanges.Keys.Last() : null;
        }
    }
}

