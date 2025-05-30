using System;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
///     Represents a belief in the GOAP system, defining a condition and an optional observed location.
///     Beliefs are used to evaluate the agent's current knowledge or world state.
/// </summary>
public class AgentBelief
{
    protected Func<bool> condition = () => false;
    [JsonIgnore]
    protected Func<Vector3> observedLocation = () => Vector3.zero;
    
    public string Name { get; }
    
    public string Description { get; set; }
    public Vector3 Location => observedLocation();

    /// <summary>
    ///     Private constructor to enforce the use of the Builder pattern for creating beliefs.
    /// </summary>
    /// <param name="name">The name of the belief.</param>
    protected AgentBelief(string name)
    {
        Name = name;
    }
    
    /// <summary>
    ///     Evaluates the belief's condition to determine its truth value.
    /// </summary>
    /// <returns>True if the belief's condition evaluates to true, false otherwise.</returns>
    public virtual bool Evaluate()
    {
        return condition();
    }

    /// <summary>
    ///     Sets a new condition for the belief.
    ///</summary>
    /// <param name="condition">The condition function to evaluate the belief's truth value.</param>
    public void NewCondition(Func<bool> condition)
    {
        this.condition = condition;
    }

    /// <summary>
    ///     Builder class for constructing instances of `AgentBelief`.
    /// </summary>
    public class Builder
    {
        private readonly AgentBelief _belief;

        /// <summary>
        ///     Initializes a new Builder with the specified belief name.
        /// </summary>
        /// <param name="name">The name of the belief.</param>
        public Builder(string name)
        {
            _belief = new AgentBelief(name);
        }
        
        public Builder WithDescription(string description)
        {
            _belief.Description = description;
            return this;
        }
        
        /// <summary>
        ///     Sets the condition function for the belief.
        /// </summary>
        /// <param name="condition">The condition function to evaluate the belief's truth value.</param>
        /// <returns>The Builder instance, for method chaining.</returns>
        public Builder WithCondition(Func<bool> condition)
        {
            _belief.condition = condition;
            return this;
        }

        /// <summary>
        ///     Sets the observed location function for the belief.
        /// </summary>
        /// <param name="observedLocation">A function returning the belief's observed location.</param>
        /// <returns>The Builder instance, for method chaining.</returns>
        public Builder WithLocation(Func<Vector3> observedLocation)
        {
            _belief.observedLocation = observedLocation;
            return this;
        }

        /// <summary>
        ///     Finalizes the belief construction and returns the resulting `AgentBelief` instance.
        /// </summary>
        /// <returns>The constructed `AgentBelief`.</returns>
        public virtual AgentBelief Build()
        {
            return _belief;
        }
    }
}