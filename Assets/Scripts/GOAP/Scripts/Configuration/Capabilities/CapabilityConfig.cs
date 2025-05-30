using System.Collections.Generic;
using GOAP.Configuration;
using GOAP.Scripts.AgentStats;

namespace GOAP.Scripts.Configuration.Capabilities
{
    /// <summary>
    /// Concrete implementation of <see cref="ICapabilityConfig"/> that encapsulates
    /// the configuration for a capability, including goals, actions, sensors, and stat configurations.
    /// </summary>
    public class CapabilityConfig : ICapabilityConfig
    {
        private readonly string capabilityName;
        private readonly List<AgentGoal> goals;
        private readonly List<AgentAction> actions;
        private readonly List<ISensor> sensors;
        private readonly List<IStatConfig> statConfigs;

        /// <summary>
        /// Initializes a new instance of the <see cref="CapabilityConfig"/> class.
        /// </summary>
        /// <param name="capabilityName">The name of the capability.</param>
        /// <param name="goals">The list of goals associated with the capability.</param>
        /// <param name="actions">The list of actions associated with the capability.</param>
        /// <param name="sensors">The list of sensors associated with the capability.</param>
        /// <param name="statConfigs">The list of stat configurations associated with the capability.</param>
        public CapabilityConfig(
            string capabilityName, 
            List<AgentGoal> goals, 
            List<AgentAction> actions, 
            List<ISensor> sensors, 
            List<IStatConfig> statConfigs)
        {
            this.capabilityName = capabilityName;
            this.goals = goals;
            this.actions = actions;
            this.sensors = sensors;
            this.statConfigs = statConfigs;
        }

        /// <summary>
        /// Configures the given <see cref="GoapAgent"/> by adding goals, actions, sensors,
        /// and registering stat configurations.
        /// </summary>
        /// <param name="agent">The GOAP agent to configure.</param>
        public void Configure(GoapAgent agent)
        {
            foreach (var goal in goals) 
                agent.goals.Add(goal);
            foreach (var action in actions) 
                agent.actions.Add(action);
            foreach (var sensor in sensors) 
                agent.sensors.Add(sensor);
            
            // Register stat configurations with the agent's StatSystem.
            foreach (var statConfig in statConfigs)
            {
                statConfig.Configure(agent.GetComponent<StatSystem>());
            }
        }
    }
}
