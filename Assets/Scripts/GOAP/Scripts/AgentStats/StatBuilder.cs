using GOAP.Scripts.AgentStats.Strategies;

namespace GOAP.Scripts.AgentStats
{
    /// <summary>
    /// Provides a fluent interface for constructing <see cref="Stat"/> instances with custom configurations.
    /// </summary>
    public class StatBuilder
    {
        private string key;
        private float initialValue = 100f;
        private float lowThreshold = 10f;
        private float highThreshold = 50f;
        private float holdDuration = 5f;

        private IStatDecayStrategy decayStrategy = new NoDecayStrategy();
        private IStatIncreaseStrategy increaseStrategy = new StandardIncreaseStrategy();
        private IStatDecreaseStrategy decreaseStrategy = new StandardDecreaseStrategy();

        /// <summary>
        /// Sets the key for the stat.
        /// </summary>
        /// <param name="key">The unique key of the stat.</param>
        /// <returns>The builder instance.</returns>
        public StatBuilder SetKey(string key)
        {
            this.key = key;
            return this;
        }

        /// <summary>
        /// Sets the initial value for the stat.
        /// </summary>
        /// <param name="value">The initial value.</param>
        /// <returns>The builder instance.</returns>
        public StatBuilder SetInitialValue(float value)
        {
            this.initialValue = value;
            return this;
        }

        /// <summary>
        /// Sets the low and high thresholds for the stat.
        /// </summary>
        /// <param name="low">The low threshold.</param>
        /// <param name="high">The high threshold.</param>
        /// <returns>The builder instance.</returns>
        public StatBuilder SetThresholds(float low, float high)
        {
            this.lowThreshold = low;
            this.highThreshold = high;
            return this;
        }

        /// <summary>
        /// Sets the duration to hold the stat value before decay.
        /// </summary>
        /// <param name="duration">The hold duration.</param>
        /// <returns>The builder instance.</returns>
        public StatBuilder SetHoldDuration(float duration)
        {
            this.holdDuration = duration;
            return this;
        }

        /// <summary>
        /// Sets the decay strategy for the stat.
        /// </summary>
        /// <param name="strategy">The decay strategy.</param>
        /// <returns>The builder instance.</returns>
        public StatBuilder SetDecayStrategy(IStatDecayStrategy strategy)
        {
            this.decayStrategy = strategy;
            return this;
        }

        /// <summary>
        /// Sets the increase strategy for the stat.
        /// </summary>
        /// <param name="strategy">The increase strategy.</param>
        /// <returns>The builder instance.</returns>
        public StatBuilder SetIncreaseStrategy(IStatIncreaseStrategy strategy)
        {
            this.increaseStrategy = strategy;
            return this;
        }

        /// <summary>
        /// Sets the decrease strategy for the stat.
        /// </summary>
        /// <param name="strategy">The decrease strategy.</param>
        /// <returns>The builder instance.</returns>
        public StatBuilder SetDecreaseStrategy(IStatDecreaseStrategy strategy)
        {
            this.decreaseStrategy = strategy;
            return this;
        }

        /// <summary>
        /// Builds and returns a new <see cref="Stat"/> instance based on the current configuration.
        /// </summary>
        /// <returns>A configured <see cref="Stat"/> instance.</returns>
        public Stat Build()
        {
            return new Stat(key, initialValue, lowThreshold, highThreshold, 
                decayStrategy, increaseStrategy, decreaseStrategy, holdDuration);
        }
    }
}
