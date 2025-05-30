using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP.Scripts
{
    /// <summary>
    ///     A factory class for creating and managing beliefs for a GOAP agent.
    ///     Provides methods for defining various types of beliefs based on conditions, sensors, and locations.
    /// </summary>
    public class BeliefFactory
    {
        private readonly GoapAgent agent;
        private readonly Dictionary<string, AgentBelief> beliefs;

        /// <summary>
        ///     Initializes the BeliefFactory with the associated GOAP agent and a dictionary to store beliefs.
        /// </summary>
        /// <param name="agent">The GOAP agent associated with this factory.</param>
        /// <param name="beliefs">The dictionary for storing generated beliefs.</param>
        public BeliefFactory(GoapAgent agent, Dictionary<string, AgentBelief> beliefs)
        {
            this.agent = agent;
            this.beliefs = beliefs;
        }

        /// <summary>
        ///     Adds a belief with a custom condition to the agent's beliefs.
        /// </summary>
        /// <param name="key">The unique key identifying the belief.</param>
        /// <param name="condition">The condition function determining the belief's truth value.</param>
        public void AddBelief(string key, Func<bool> condition)
        {
            if (beliefs.ContainsKey(key)) return;
            beliefs.Add(key, new AgentBelief.Builder(key).WithCondition(condition).Build());
        }

        /// <summary>
        ///     Removes a belief from the agent's belief dictionary using the specified key.
        /// </summary>
        /// <param name="key">The unique key identifying the belief to remove.</param>
        public void RemoveBelief(string key)
        {
            beliefs.Remove(key);
        }

        /// <summary>
        ///     Adds a belief based on a sensor's state and target position.
        /// </summary>
        /// <param name="key">The unique key identifying the belief.</param>
        /// <param name="sensor">The sensor to monitor for target range and position.</param>
        public void AddSensorBelief<T>(string key, Sensor<T> sensor) where T : Component
        {
            beliefs.Add(key, new AgentBelief.Builder(key)
                .WithCondition(() => sensor.IsTargetInRange)
                .WithLocation(() => sensor.TargetPosition)
                .Build());
        }

        public void AddSensorBelief<T>(string key, TargetSensor<T> sensor) where T : Component
        {
            beliefs.Add(key, new AgentBelief.Builder(key)
                .WithCondition(() => sensor.IsTargetInRange)
                .WithLocation(() => sensor.TargetPosition)
                .Build());
        }

        public void AddSensorBelief<T>(string key, TargetSensorWithStrategy<T> sensor) where T : Component
        {
            beliefs.Add(key, new AgentBelief.Builder(key)
                .WithCondition(() => sensor.IsTargetInRange)
                .WithLocation(() => sensor.TargetPosition)
                .Build());
        }


        /// <summary>
        ///     Adds a belief based on proximity to a specified location, defined by a Transform.
        /// </summary>
        /// <param name="key">The unique key identifying the belief.</param>
        /// <param name="distance">The maximum distance for the belief to evaluate as true.</param>
        /// <param name="locationCondition">The target location (as a Transform).</param>
        public void AddLocationBelief(string key, float distance, Transform locationCondition)
        {
            AddLocationBelief(key, distance, locationCondition.position);
        }

        /// <summary>
        ///     Adds a belief based on proximity to a specified location, defined by a Vector3 position.
        /// </summary>
        /// <param name="key">The unique key identifying the belief.</param>
        /// <param name="distance">The maximum distance for the belief to evaluate as true.</param>
        /// <param name="locationCondition">The target location (as a Vector3).</param>
        public void AddLocationBelief(string key, float distance, Vector3 locationCondition)
        {
            beliefs.Add(key, new AgentBelief.Builder(key)
                .WithCondition(() => InRangeOf(locationCondition, distance))
                .WithLocation(() => locationCondition)
                .Build());
        }

        /// <summary>
        ///     Determines whether the agent is within a specified range of a given position.
        /// </summary>
        /// <param name="pos">The target position to check.</param>
        /// <param name="range">The range to evaluate.</param>
        /// <returns>True if the agent is within range, false otherwise.</returns>
        private bool InRangeOf(Vector3 pos, float range)
        {
            return Vector3.Distance(agent.transform.position, pos) < range;
        }
        

        public void AddBeliefWithPrediction(string key, Func<bool> condition, Func<IEnumerator> coroutine,
            string description = "")
        {
            if (beliefs.ContainsKey(key)) return;
            beliefs.Add(key, new AgentBeliefPrediction.Builder(key)
                .WithCondition(condition)
                .WithDescription(description).
                WithPredictionCoroutine(coroutine).Build());
        }
    }
}