using UnityEngine;

namespace GOAP.Scripts.AgentStats.Strategies
{
    /// <summary>
    /// Defines a strategy for increasing a stat's value.
    /// </summary>
    public interface IStatIncreaseStrategy
    {
        /// <summary>
        /// Applies an increase to the current stat value.
        /// </summary>
        /// <param name="currentValue">The current value of the stat.</param>
        /// <param name="increaseAmount">The amount to increase by.</param>
        /// <param name="maxValue">The maximum allowable value.</param>
        /// <returns>The new stat value after applying the increase.</returns>
        float ApplyIncrease(float currentValue, float increaseAmount, float maxValue);
    }
    
    /// <summary>
    /// A standard implementation that increases the stat value without exceeding the maximum.
    /// </summary>
    public class StandardIncreaseStrategy : IStatIncreaseStrategy
    {
        public float ApplyIncrease(float currentValue, float increaseAmount, float maxValue)
        {
            return Mathf.Min(currentValue + increaseAmount, maxValue);
        }
    }
    
    /// <summary>
    /// Increases the stat value only if it is below a specified cap threshold.
    /// </summary>
    public class CappedIncreaseStrategy : IStatIncreaseStrategy
    {
        private float capThreshold;

        public CappedIncreaseStrategy(float capThreshold)
        {
            this.capThreshold = capThreshold;
        }

        public float ApplyIncrease(float currentValue, float increaseAmount, float maxValue)
        {
            if (currentValue >= capThreshold) return currentValue;
            return Mathf.Min(currentValue + increaseAmount, maxValue);
        }
    }
    
    /// <summary>
    /// Allows the stat to be increased beyond its maximum up to an overcharge limit.
    /// </summary>
    public class OverchargeIncreaseStrategy : IStatIncreaseStrategy
    {
        private float overchargeLimit;
        private float decayRate;

        public OverchargeIncreaseStrategy(float overchargeLimit, float decayRate)
        {
            this.overchargeLimit = overchargeLimit;
            this.decayRate = decayRate;
        }

        public float ApplyIncrease(float currentValue, float increaseAmount, float maxValue)
        {
            float newValue = currentValue + increaseAmount;
            return Mathf.Min(newValue, maxValue + overchargeLimit);
        }
    }
    
    /// <summary>
    /// Gradually increases the stat value over time.
    /// </summary>
    public class GradualIncreaseStrategy : IStatIncreaseStrategy
    {
        private float speed; // Rate of increase per second

        public GradualIncreaseStrategy(float speed)
        {
            this.speed = speed;
        }

        public float ApplyIncrease(float currentValue, float increaseAmount, float maxValue)
        {
            return Mathf.Min(currentValue + (increaseAmount * Time.deltaTime * speed), maxValue);
        }
    }
}
