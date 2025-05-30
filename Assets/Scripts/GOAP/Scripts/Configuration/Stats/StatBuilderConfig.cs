using GOAP.Scripts.AgentStats;
using GOAP.Scripts.AgentStats.Strategies;

namespace GOAP.Configuration
{
    public class StatBuilderConfig
    {
        private readonly StatBuilder builder = new();

        public StatBuilderConfig SetKey(string key)
        {
            builder.SetKey(key);
            return this;
        }

        public StatBuilderConfig SetInitialValue(float value)
        {
            builder.SetInitialValue(value);
            return this;
        }

        public StatBuilderConfig SetThresholds(float low, float high)
        {
            builder.SetThresholds(low, high);
            return this;
        }

        public StatBuilderConfig SetHoldDuration(float duration)
        {
            builder.SetHoldDuration(duration);
            return this;
        }

        public StatBuilderConfig SetDecayStrategy(IStatDecayStrategy strategy)
        {
            builder.SetDecayStrategy(strategy);
            return this;
        }

        public StatBuilderConfig SetIncreaseStrategy(IStatIncreaseStrategy strategy)
        {
            builder.SetIncreaseStrategy(strategy);
            return this;
        }

        public StatBuilderConfig SetDecreaseStrategy(IStatDecreaseStrategy strategy)
        {
            builder.SetDecreaseStrategy(strategy);
            return this;
        }

        public IStatConfig Build()
        {
            return new StatConfig(builder.Build());
        }
    }
}