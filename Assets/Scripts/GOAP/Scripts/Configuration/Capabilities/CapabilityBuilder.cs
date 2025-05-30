using System;
using System.Collections.Generic;
using GOAP.Configuration;
using UnityEngine;

namespace GOAP.Scripts.Configuration.Capabilities
{
    /// <summary>
    /// Provides a fluent builder for constructing an <see cref="ICapabilityConfig"/> instance.
    /// Allows adding goals, actions, sensors, stat configurations, and setting a base cost.
    /// </summary>
    public class CapabilityBuilder
    {
        private readonly string capabilityName;
        private readonly List<IStatConfig> statConfigs = new();
        private readonly List<AgentGoal> goals = new();
        private readonly List<AgentAction> actions = new();
        private readonly List<ISensor> sensors = new();
        private float baseCost;

        /// <summary>
        /// Initializes a new instance of the <see cref="CapabilityBuilder"/> class with the specified capability name.
        /// </summary>
        /// <param name="capabilityName">The unique name of the capability.</param>
        public CapabilityBuilder(string capabilityName)
        {
            this.capabilityName = capabilityName;
        }

        /// <summary>
        /// Adds a goal to the capability using a goal factory function.
        /// </summary>
        /// <param name="goalFactory">A function that creates an <see cref="AgentGoal"/> instance.</param>
        /// <returns>The builder instance for chaining.</returns>
        public CapabilityBuilder AddGoal(Func<AgentGoal> goalFactory)
        {
            goals.Add(goalFactory());
            return this;
        }

        /// <summary>
        /// Adds an action to the capability using an action factory function.
        /// The action's cost is set to the builder's base cost.
        /// </summary>
        /// <param name="actionFactory">A function that creates an <see cref="AgentAction"/> instance.</param>
        /// <returns>The builder instance for chaining.</returns>
        public CapabilityBuilder AddAction(Func<AgentAction> actionFactory)
        {
            var action = actionFactory();
            action.Cost = baseCost;
            actions.Add(action);
            return this;
        }

        /// <summary>
        /// Adds a sensor to the capability using a sensor factory function.
        /// </summary>
        /// <typeparam name="T">The type of Component the sensor monitors.</typeparam>
        /// <param name="sensorFactory">A function that creates an <see cref="Sensor{T}"/> instance.</param>
        /// <returns>The builder instance for chaining.</returns>
        public CapabilityBuilder AddSensor<T>(Func<Sensor<T>> sensorFactory) where T : Component
        {
            sensors.Add(sensorFactory());
            return this;
        }

        /// <summary>
        /// Adds a stat configuration to the capability using a stat factory function.
        /// </summary>
        /// <param name="statFactory">A function that creates an <see cref="IStatConfig"/> instance.</param>
        /// <returns>The builder instance for chaining.</returns>
        public CapabilityBuilder AddStat(Func<IStatConfig> statFactory)
        {
            statConfigs.Add(statFactory());
            return this;
        }

        /// <summary>
        /// Sets the base cost for actions within this capability.
        /// </summary>
        /// <param name="cost">The base cost value.</param>
        /// <returns>The builder instance for chaining.</returns>
        public CapabilityBuilder SetBaseCost(float cost)
        {
            baseCost = cost;
            return this;
        }

        /// <summary>
        /// Builds and returns the configured <see cref="ICapabilityConfig"/> instance.
        /// </summary>
        /// <returns>A new <see cref="ICapabilityConfig"/> instance representing this capability configuration.</returns>
        public ICapabilityConfig Build()
        {
            return new CapabilityConfig(capabilityName, goals, actions, sensors, statConfigs);
        }
    }
}
