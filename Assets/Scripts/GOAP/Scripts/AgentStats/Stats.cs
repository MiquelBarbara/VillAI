using GOAP.Scripts.AgentStats.Strategies;

namespace GOAP.Scripts.AgentStats
{
    /// <summary>
    /// Represents a single stat with current value, thresholds, and strategies for decay, increase, and decrease.
    /// </summary>
    public class Stat
    {
        private float maxValue;
        private float value;
        private float lastUpdateTime;
        private CountdownTimer holdTimer;

        private IStatDecayStrategy decayStrategy;
        private IStatIncreaseStrategy increaseStrategy;
        private IStatDecreaseStrategy decreaseStrategy;

        /// <summary>
        /// The high threshold value of the stat.
        /// </summary>
        public float thresholdHigh;

        /// <summary>
        /// The low threshold value of the stat.
        /// </summary>
        public float thresholdLow;

        /// <summary>
        /// The duration for which the stat's value is held before decay.
        /// </summary>
        public float holdDuration;

        /// <summary>
        /// Gets the key (name) of the stat.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stat"/> class.
        /// </summary>
        /// <param name="key">The unique key identifying the stat.</param>
        /// <param name="initialValue">The initial value of the stat.</param>
        /// <param name="lowThreshold">The low threshold for the stat.</param>
        /// <param name="highThreshold">The high threshold for the stat.</param>
        /// <param name="decayStrategy">The decay strategy to apply over time.</param>
        /// <param name="increaseStrategy">The strategy to apply when increasing the stat.</param>
        /// <param name="decreaseStrategy">The strategy to apply when decreasing the stat.</param>
        /// <param name="holdDuration">The duration to hold the stat value before decay starts.</param>
        public Stat(string key, float initialValue, float lowThreshold, float highThreshold, 
            IStatDecayStrategy decayStrategy, IStatIncreaseStrategy increaseStrategy, 
            IStatDecreaseStrategy decreaseStrategy, float holdDuration = 5f)
        {
            Key = key;
            value = initialValue;
            maxValue = initialValue;
            thresholdLow = lowThreshold;
            thresholdHigh = highThreshold;
            this.decayStrategy = decayStrategy;
            this.increaseStrategy = increaseStrategy;
            this.decreaseStrategy = decreaseStrategy;
            this.holdDuration = holdDuration;
            lastUpdateTime = 0f;

            // Initialize the timer with the hold duration.
            holdTimer = new CountdownTimer(holdDuration);
        }

        /// <summary>
        /// Gets the current value of the stat.
        /// </summary>
        public float Value => value;

        /// <summary>
        /// Updates the stat by applying decay over the elapsed time.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last update.</param>
        public void Update(float deltaTime)
        {
            // Apply decay to the stat's value.
            value = decayStrategy.ApplyDecay(value, deltaTime);
            lastUpdateTime += deltaTime;
        }

        /// <summary>
        /// Increases the stat's value by a specified amount and restarts the hold timer.
        /// </summary>
        /// <param name="amount">The amount to increase the stat by.</param>
        public void Increase(float amount)
        {
            float oldValue = value;
            value = increaseStrategy.ApplyIncrease(value, amount, maxValue);
            holdTimer.Reset();
            holdTimer.Start();
        }

        /// <summary>
        /// Decreases the stat's value by a specified amount.
        /// </summary>
        /// <param name="amount">The amount to decrease the stat by.</param>
        public void Decrease(float amount)
        {
            float oldValue = value;
            value = decreaseStrategy.ApplyDecrease(value, amount, 0);
        }

        /// <summary>
        /// Gets a value indicating whether the stat is considered high.
        /// </summary>
        public bool IsHigh => value >= thresholdHigh;

        /// <summary>
        /// Gets a value indicating whether the stat is considered low.
        /// </summary>
        public bool IsLow => value <= thresholdLow;

        /// <summary>
        /// Gets a value indicating whether the stat is in the normal range.
        /// </summary>
        public bool IsNormal => value >= thresholdLow && value < thresholdHigh;
    }
}
