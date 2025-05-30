using System.Collections.Generic;

namespace GOAP.Scripts
{
    /// <summary>
    ///     Represents an individual action in the GOAP system.
    ///     Actions have preconditions, effects, a cost, and a strategy to execute the behavior.
    /// </summary>
    public class AgentAction
    {
        /// <summary>
        ///     The strategy that defines how the action is executed in-game.
        /// </summary>
        private IActionStrategy strategy;

        /// <summary>
        ///     Initializes a new action with a specific name.
        /// </summary>
        /// <param name="name">The name of the action.</param>
        public AgentAction(string name)
        {
            Name = name;
        }

        /// <summary>
        ///     The name of the action.
        /// </summary>
        public string Name { get; }
        
        public string Description { get; set; }
        /// <summary>
        ///     The cost of performing the action, used for optimization in planning.
        /// </summary>
        public float Cost { get; set; }

        /// <summary>
        ///     The set of conditions that must be true before the action can be executed.
        /// </summary>
        public HashSet<AgentBelief> Preconditions { get; } = new();

        /// <summary>
        ///     The set of conditions that will be true after the action is successfully executed.
        /// </summary>
        public HashSet<AgentBelief> Effects { get; } = new();

        /// <summary>
        ///     Indicates whether the action has been completed.
        /// </summary>
        public bool Complete => strategy.Complete;

        /// <summary>
        ///     Starts executing the action by invoking its strategy.
        /// </summary>
        public void Start()
        {
            strategy.Start();
        }

        /// <summary>
        ///     Updates the action's progress based on the provided delta time.
        ///     If the action completes, its effects are applied to the system.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last update.</param>
        public void Update(float deltaTime)
        {
            if (strategy.CanPerform) strategy.Update(deltaTime);

            if (!strategy.Complete) return;

            foreach (var effect in Effects) effect.Evaluate();
        }

        /// <summary>
        ///     Stops the action's execution and cleans up its state.
        /// </summary>
        public void Stop()
        {
            strategy.Stop();
        }

        /// <summary>
        ///     A builder class for creating instances of `AgentAction` with a fluent interface.
        /// </summary>
        public class Builder
        {
            private readonly AgentAction _action;

            /// <summary>
            ///     Initializes the builder with the name of the action.
            /// </summary>
            /// <param name="name">The name of the action.</param>
            public Builder(string name)
            {
                _action = new AgentAction(name)
                {
                    Cost = 1 // Default cost
                };
            }
            
            public Builder WithDescription(string description)
            {
                _action.Description = description;
                return this;
            }

            /// <summary>
            ///     Sets the cost of the action.
            /// </summary>
            /// <param name="cost">The cost of the action.</param>
            /// <returns>The builder instance.</returns>
            public Builder WithCost(float cost)
            {
                _action.Cost = cost;
                return this;
            }

            /// <summary>
            ///     Sets the strategy to define how the action is performed.
            /// </summary>
            /// <param name="strategy">The strategy instance.</param>
            /// <returns>The builder instance.</returns>
            public Builder WithStrategy(IActionStrategy strategy)
            {
                _action.strategy = strategy;
                return this;
            }

            /// <summary>
            ///     Adds a precondition to the action.
            /// </summary>
            /// <param name="precondition">The belief representing the precondition.</param>
            /// <returns>The builder instance.</returns>
            public Builder AddPrecondition(AgentBelief precondition)
            {
                _action.Preconditions.Add(precondition);
                return this;
            }

            /// <summary>
            ///     Adds an effect to the action.
            /// </summary>
            /// <param name="effect">The belief representing the effect.</param>
            /// <returns>The builder instance.</returns>
            public Builder AddEffect(AgentBelief effect)
            {
                _action.Effects.Add(effect);
                return this;
            }

            /// <summary>
            ///     Builds and returns the configured `AgentAction` instance.
            /// </summary>
            /// <returns>The constructed `AgentAction`.</returns>
            public AgentAction Build()
            {
                return _action;
            }
        }
    }
}