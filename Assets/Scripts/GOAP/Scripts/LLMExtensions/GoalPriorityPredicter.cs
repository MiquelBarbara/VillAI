using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.ALPHA;
using UnityEngine;
using GOAP.Scripts;
using LLM.Services;
using LLM.Templates;
using UnityServiceLocator; // for AgentGoal


/// <summary>
/// Updates the priorities of goals for a GOAP agent by retrieving predictions from an external service.
/// </summary>
public class GoalPriorityPredicter
{
    private readonly List<AgentGoal> _goals;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="GoalPriorityPredicter"/> class using the agent's current goals.
    /// </summary>
    /// <param name="agent">The GOAP agent whose goals will be updated.</param>
    public GoalPriorityPredicter(GoapAgent agent)
    {
        _goals = agent.goals.ToList();
    }
    
    /// <summary>
    /// Asynchronously updates the goal priorities for the agent using a prediction service.
    /// </summary>
    /// <param name="agent">The GOAP agent whose goal priorities will be updated.</param>
    /// <returns>An IEnumerator for coroutine execution.</returns>
    public IEnumerator UpdateGoalPriorities(GoapAgent agent)
    {
        var templateData = new GoapGoalPriorityData
        {
            objectives = _goals
        };
        
        ServiceLocator.Global.Get<IPredictionService>(out var service);
        //yield return service.Predict(templateData);
        
        
        if (templateData.priorities == null || templateData.priorities.Count != _goals.Count)
            yield break;
        
        for (var i = 0; i < templateData.priorities.Count; i++)
        {
            agent.goals.First(goal => goal.Name == _goals[i].Name).UpdatePriority(templateData.priorities[i]);
        }
    }
}
