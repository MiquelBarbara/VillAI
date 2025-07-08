using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using LLM.Services;
using LLM.Templates;
using NPCs;
using UnityEngine;
using UnityServiceLocator;
using Utilities.ScriptableObjectExtensions;

namespace GOAP.Scripts
{

    public class GOAPPlanData: DataTransferObject
    {
        [Input] public List<AgentGoal> availableGoals;
        [Input] public List<AgentAction> availableActions;
        [Input] public Dictionary<string, AgentBelief> agentBeliefs;
        [Input] public Queue<ActionPlan> actualPlans;

        [Output] public List<string> plan;
        [Output] public string goal;
    }
    
    public class GOAPGoalData: DataTransferObject
    {
        [Input] public List<AgentGoal> objectives;
        [Input] public ComplexCharacterData character;

        [Output] public List<int> priorities;
    }
    
    public class GOAPBeliefData: DataTransferObject
    {
        [Input] public AgentBelief evaluatedBelief;
        
        [Output] public bool boolean;
    }
    /// <summary>
    ///     Implementation of a GOAP planner. Determines a sequence of actions that allow an agent
    ///     to achieve a prioritized set of goals by fulfilling the required effects.
    /// </summary>
    public class AIGoapPlanner
    {
        
        private Queue<ActionPlan> ActionPlans { get; } = new Queue<ActionPlan>();
        
        public ActionPlan Plan(GoapAgent agent, HashSet<AgentGoal> goals)
        {
           if(ActionPlans.Count <= 0){
               DefaultPlan(agent, goals);
           }
           return ActionPlans.Dequeue();
        }
        
        public void AddPlan(ActionPlan actionPlan)
        {
            ActionPlans.Enqueue(actionPlan);
        }
        
        public void ClearPlans()
        {
            ActionPlans.Clear();
        }

        private void DefaultPlan(GoapAgent agent, HashSet<AgentGoal> goals)
        {
            var actions = new Stack<AgentAction>();
            actions.Push(agent.actions.FirstOrDefault(action => action.Name == "Wander Around"));
            
            var actionPlan = new ActionPlan(goals.FirstOrDefault(goal => goal.Name == "Wander"),
                actions, 0);
            ActionPlans.Enqueue(actionPlan);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="goals"></param>
        /// <returns></returns>
        public IEnumerator CheckGoalsPriority(GoapAgent agent, HashSet<AgentGoal> goals)
        {
            var goalData = new GOAPGoalData
            {
                objectives = goals.ToList(),
                character = agent.GetComponent<ResourceLocator>().Get<ComplexCharacterData>()
            };

            yield return ServiceLocator.Global.Get<IPredictionService>().Predict(GOAPPromptProvider.Instance.goalPrompt, goalData);

            if (goalData.priorities == null || goalData.priorities.Count <= 0) yield break;

            for(int i = 0; i < goalData.objectives.Count; i++)
            {
                var goal = goalData.objectives[i];
                if (goal == null) continue;
                goal.UpdatePriority(goalData.priorities[i]);
            }
        }
        
        public IEnumerator CheckBeliefs(GoapAgent agent, Dictionary<string, AgentBelief> beliefs)
        {
            var beliefList = beliefs.Values.ToList();
            beliefList.Shuffle();
            foreach (var belief in beliefList)
            {
                if (belief is AgentBeliefPrediction beliefPrediction)
                {
                    yield return agent.StartCoroutine(beliefPrediction.PredictionCoroutine);
                }
            }
        }
        
        /// <summary>
        /// Predicts a plan for the agent based on the available goals and actions.
        /// </summary>
        /// <param name="agent"> The agent for which the plan is being predicted.</param>
        /// <param name="goals"> The set of goals available to the agent.</param>
        /// <param name="mostRecentGoal"> The most recent goal that the agent has been working towards, if any.</param>
        /// <returns> An enumerator that allows for asynchronous operation, yielding control back to the caller until the prediction is complete.</returns>
        public IEnumerator PredictPlan(GoapAgent agent, HashSet<AgentGoal> goals, AgentGoal mostRecentGoal = null)
        {
            // Data transfer object for the GOAP plan prediction.
            var planData = new GOAPPlanData
            {
                availableGoals = goals.ToList(),
                availableActions = agent.actions.ToList(),
                agentBeliefs = agent.beliefs,
                actualPlans = ActionPlans
            };

            yield return ServiceLocator.Global.Get<IPredictionService>().Predict(GOAPPromptProvider.Instance.planPrompt, planData);

            // Check if the plan was successfully generated.
            if (planData.plan == null || planData.plan.Count <= 0) yield break;
            var actions = new Stack<AgentAction>();
            var totalCost = 0f;
            // Iterate through the action names in the plan and find the corresponding actions in the agent's action set.
            foreach (var action in planData.plan.Select(actionName => agent.actions.FirstOrDefault(a => a.Name == actionName)).Where(action => action != null))
            {
                totalCost += action.Cost;
                actions.Push(action);
            }
            // Get the goal from the plan data, or use the most recent goal if it is not specified.
            var goal = goals.FirstOrDefault(g => g.Name == planData.goal) ?? mostRecentGoal;
            if (goal == null) yield break;
            // Create a new action plan with the predicted goal, actions, and total cost.
            var actionPlan = new ActionPlan(goal, actions, totalCost);
            // Add the action plan to the planner's queue of plans.
            AddPlan(actionPlan);
        }
    }
}