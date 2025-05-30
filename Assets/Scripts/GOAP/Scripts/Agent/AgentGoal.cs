using System.Collections.Generic;

namespace GOAP.Scripts
{
    /// <summary>
    ///     Represents a goal for a GOAP agent, encapsulating its name, priority, and desired effects.
    ///     Goals define what the agent aims to achieve, with conditions that describe the desired world state.
    /// </summary>
    public class AgentGoal
    {
        /// <summary>
        ///     Initializes a new instance of the `AgentGoal` class with the specified name.
        /// </summary>
        /// <param name="name">The name of the goal.</param>
        private AgentGoal(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public string Description { get; set; }
        public float Priority { get; private set; }

        /// <summary>
        ///     A set of desired effects that must be achieved for the goal to be considered fulfilled.
        /// </summary>
        public HashSet<AgentBelief> DesiredEffects { get; } = new();
        
        /// <summary>
        ///     Updates the priority of the goal.
        /// </summary>
        /// <param name="priority"></param>
        public void UpdatePriority(float priority) => Priority = priority;

        /// <summary>
        ///     Builder class for constructing instances of `AgentGoal`.
        /// </summary>
        public class Builder
        {
            private readonly AgentGoal goal;

            /// <summary>
            ///     Initializes a new Builder with the specified goal name.
            /// </summary>
            /// <param name="name">The name of the goal.</param>
            public Builder(string name)
            {
                goal = new AgentGoal(name);
            }

            /// <summary>
            ///     Sets the priority of the goal.
            /// </summary>
            /// <param name="priority">The priority value.</param>
            /// <returns>The Builder instance, for method chaining.</returns>
            public Builder WithPriority(float priority)
            {
                goal.Priority = priority;
                return this;
            }
            
            public Builder WithDescription(string description)
            {
                goal.Description = description;
                return this;
            }

            /// <summary>
            ///     Adds a desired effect to the goal's desired effects.
            /// </summary>
            /// <param name="effect">The desired effect to add.</param>
            /// <returns>The Builder instance, for method chaining.</returns>
            public Builder WithDesiredEffect(AgentBelief effect)
            {
                goal.DesiredEffects.Add(effect);
                return this;
            }

            /// <summary>
            ///     Finalizes the goal construction and returns the resulting `AgentGoal` instance.
            /// </summary>
            /// <returns>The constructed `AgentGoal`.</returns>
            public AgentGoal Build()
            {
                return goal;
            }
        }
    }
}