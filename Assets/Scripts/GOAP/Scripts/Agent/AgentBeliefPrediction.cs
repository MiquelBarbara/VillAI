using System;
using System.Collections;
using UnityEngine;

namespace GOAP.Scripts
{
    /// <summary>
    ///    Represents a belief that can use the prediction system to update values about the condition of the belief.
    /// </summary>
    public class AgentBeliefPrediction: AgentBelief
    {
        Func<IEnumerator> coroutine;
        public IEnumerator PredictionCoroutine => coroutine();
        protected AgentBeliefPrediction(string name) : base(name)
        {
        }
        
        public class Builder
        {
            private readonly AgentBeliefPrediction _belief;
            
            /// <summary>
            ///     Builds an instance of `AgentBeliefPrediction` with the specified name.
            /// </summary>
            /// <returns>A new instance of `AgentBeliefPrediction`.</returns>
            public  AgentBelief Build()
            {
                return _belief;
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
            
            public Builder WithPredictionCoroutine(Func<IEnumerator> coroutine)
            {
                _belief.coroutine = coroutine;
                return this;
            }

            public Builder(string name)
            {
                _belief = new AgentBeliefPrediction(name);
            }
        }
    }
}