using UnityEngine;

namespace GOAP.Scripts.AgentStats.Strategies
{
    /// <summary>
    /// Defines a strategy for decaying a stat's value over time.
    /// </summary>
    public interface IStatDecayStrategy
    {
        /// <summary>
        /// Applies decay to the current stat value based on elapsed time.
        /// </summary>
        /// <param name="currentValue">The current value of the stat.</param>
        /// <param name="deltaTime">The elapsed time since the last update.</param>
        /// <returns>The new stat value after decay is applied.</returns>
        float ApplyDecay(float currentValue, float deltaTime);
    }
    
    /// <summary>
    /// Implements a linear decay strategy that subtracts a constant amount per unit time.
    /// </summary>
    public class LinearDecayStrategy : IStatDecayStrategy
    {
        private float decayRate;

        public LinearDecayStrategy(float decayRate)
        {
            this.decayRate = decayRate;
        }

        public float ApplyDecay(float currentValue, float deltaTime)
        {
            return Mathf.Max(currentValue - (decayRate * deltaTime), 0);
        }
    }
    
    /// <summary>
    /// Implements an exponential decay strategy where the value decays proportionally to its current value.
    /// </summary>
    public class ExponentialDecayStrategy : IStatDecayStrategy
    {
        private float decayFactor;

        public ExponentialDecayStrategy(float decayFactor)
        {
            this.decayFactor = decayFactor;
        }

        public float ApplyDecay(float currentValue, float deltaTime)
        {
            return Mathf.Max(currentValue * Mathf.Exp(-decayFactor * deltaTime), 0);
        }
    }
    
    /// <summary>
    /// Applies decay only if the current value is below a specified threshold.
    /// </summary>
    public class ThresholdDecayStrategy : IStatDecayStrategy
    {
        private float threshold;
        private float decayRate;

        public ThresholdDecayStrategy(float threshold, float decayRate)
        {
            this.threshold = threshold;
            this.decayRate = decayRate;
        }

        public float ApplyDecay(float currentValue, float deltaTime)
        {
            if (currentValue > threshold) return currentValue;
            return Mathf.Max(currentValue - (decayRate * deltaTime), 0);
        }
    }
    
    /// <summary>
    /// Implements a decay strategy where no decay is applied.
    /// </summary>
    public class NoDecayStrategy : IStatDecayStrategy
    {
        public float ApplyDecay(float currentValue, float deltaTime)
        {
            return currentValue;
        }
    }
}
