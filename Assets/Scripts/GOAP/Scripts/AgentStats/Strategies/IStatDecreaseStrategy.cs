using UnityEngine;

namespace GOAP.Scripts.AgentStats.Strategies
{
    /// <summary>
    /// Defines a strategy for decreasing a stat's value.
    /// </summary>
    public interface IStatDecreaseStrategy
    {
        /// <summary>
        /// Applies a decrease to the current stat value.
        /// </summary>
        /// <param name="currentValue">The current value of the stat.</param>
        /// <param name="decreaseAmount">The amount to decrease by.</param>
        /// <param name="minValue">The minimum allowable value.</param>
        /// <returns>The new stat value after applying the decrease.</returns>
        float ApplyDecrease(float currentValue, float decreaseAmount, float minValue);
    }
    
    /// <summary>
    /// A standard implementation that decreases the stat value without dropping below the minimum.
    /// </summary>
    public class StandardDecreaseStrategy : IStatDecreaseStrategy
    {
        public float ApplyDecrease(float currentValue, float decreaseAmount, float minValue)
        {
            return Mathf.Max(currentValue - decreaseAmount, minValue);
        }
    }
    
    /// <summary>
    /// Decreases the stat value by a percentage of its current value.
    /// </summary>
    public class PercentageDecreaseStrategy : IStatDecreaseStrategy
    {
        private float percentage;

        public PercentageDecreaseStrategy(float percentage)
        {
            this.percentage = percentage;
        }

        public float ApplyDecrease(float currentValue, float decreaseAmount, float minValue)
        {
            float decrease = currentValue * (percentage / 100f);
            return Mathf.Max(currentValue - decrease, minValue);
        }
    }
    
    /// <summary>
    /// Gradually decreases the stat value over time.
    /// </summary>
    public class GradualDecreaseStrategy : IStatDecreaseStrategy
    {
        private float speed; // Rate of decrease per second

        public GradualDecreaseStrategy(float speed)
        {
            this.speed = speed;
        }

        public float ApplyDecrease(float currentValue, float decreaseAmount, float minValue)
        {
            return Mathf.Max(currentValue - (decreaseAmount * Time.deltaTime * speed), minValue);
        }
    }
    
    /// <summary>
    /// Decreases the stat only if it is above a certain threshold.
    /// </summary>
    public class ThresholdDecreaseStrategy : IStatDecreaseStrategy
    {
        private float threshold;

        public ThresholdDecreaseStrategy(float threshold)
        {
            this.threshold = threshold;
        }

        public float ApplyDecrease(float currentValue, float decreaseAmount, float minValue)
        {
            if (currentValue <= threshold) return currentValue;
            return Mathf.Max(currentValue - decreaseAmount, minValue);
        }
    }
}
