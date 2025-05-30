using GOAP.Scripts;
using GOAP.Scripts.AgentStats;

namespace GOAP.Configuration
{
    public class StatConfig : IStatConfig
    {
        private readonly Stat stat;

        public StatConfig(Stat stat)
        {
            this.stat = stat;
        }

        public void Configure(StatSystem statSystem)
        {
            statSystem.RegisterStat(stat);
        }
    }
}