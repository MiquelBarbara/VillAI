using System.Collections.Generic;
using System.Linq;
using GOAP.Scripts.AgentStats.Strategies;
using UnityEngine;

namespace GOAP.Scripts.AgentStats
{
    /// <summary>
    /// Manages a collection of stats for a game entity.
    /// Provides methods to register, increase, decrease, and retrieve stats.
    /// </summary>
    public class StatSystem : MonoBehaviour
    {
        private List<Stat> _stats = new List<Stat>();

        private void Start()
        {
            var builder = new StatBuilder();

            // Example: Create a Health stat with custom thresholds and strategies.
            var healthStat = builder.SetKey("Health")
                .SetThresholds(30f, 50f)
                .SetDecayStrategy(new LinearDecayStrategy(0.1f))
                .SetIncreaseStrategy(new StandardIncreaseStrategy())
                .SetDecreaseStrategy(new StandardDecreaseStrategy())
                .Build();

            _stats.Add(healthStat);
        }

        /// <summary>
        /// Registers a new stat if it is not already present.
        /// </summary>
        /// <param name="stat">The stat to register.</param>
        public void RegisterStat(Stat stat)
        {
            if (!_stats.Exists(s => s.Key == stat.Key))
            {
                _stats.Add(stat);
            }
        }

        /// <summary>
        /// Increases the value of the stat with the given key.
        /// </summary>
        /// <param name="key">The key of the stat to increase.</param>
        /// <param name="amount">The amount to increase.</param>
        public void IncreaseStat(string key, float amount)
        {
            foreach (var stat in _stats.Where(stat => stat.Key == key))
            {
                stat.Increase(amount);
            }
        }

        /// <summary>
        /// Decreases the value of the stat with the given key.
        /// </summary>
        /// <param name="key">The key of the stat to decrease.</param>
        /// <param name="amount">The amount to decrease.</param>
        public void DecreaseStat(string key, float amount)
        {
            foreach (var stat in _stats.Where(stat => stat.Key == key))
            {
                stat.Decrease(amount);
            }
        }

        /// <summary>
        /// Returns all stats managed by the system.
        /// </summary>
        /// <returns>A list of all registered stats.</returns>
        public List<Stat> GetAllStats()
        {
            return _stats;
        }

        /// <summary>
        /// Attempts to retrieve a stat by its key.
        /// </summary>
        /// <param name="key">The key of the stat.</param>
        /// <param name="stat">When this method returns, contains the stat if found; otherwise, null.</param>
        /// <returns>True if the stat was found; otherwise, false.</returns>
        public bool TryGetStat(string key, out Stat stat)
        {
            if (_stats.Exists(s => s.Key == key))
            {
                stat = _stats.First(s => s.Key == key);
                return true;
            }
            stat = null;
            return false;
        }
    }
}
