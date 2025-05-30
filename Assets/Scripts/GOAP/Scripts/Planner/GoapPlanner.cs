using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOAP.Scripts
{
    /// <summary>
    ///     Interface defining the contract for a GOAP (Goal-Oriented Action Planning) planner.
    ///     Provides a method for generating an action plan for a given agent and set of goals.
    /// </summary>
    public interface IGoapPlanner
    {
        /// <summary>
        ///     Creates a plan to achieve the agent's goals based on available actions.
        /// </summary>
        /// <param name="agent">The agent executing the plan.</param>
        /// <param name="goals">A set of possible goals the agent can pursue.</param>
        /// <param name="mostRecentGoal">The agent's most recently pursued goal, if any.</param>
        /// <returns>An `ActionPlan` if a viable path is found, or `null` if no plan is possible.</returns>
        ActionPlan Plan(GoapAgent agent, HashSet<AgentGoal> goals, AgentGoal mostRecentGoal = null);
    }

    /// <summary>
    ///     Implementation of a GOAP planner. Determines a sequence of actions that allow an agent
    ///     to achieve a prioritized set of goals by fulfilling the required effects.
    /// </summary>
    public class GoapPlanner : IGoapPlanner
    {
        public ActionPlan Plan(GoapAgent agent, HashSet<AgentGoal> goals, AgentGoal mostRecentGoal = null)
        {
            // Order goals by priority, prioritizing the most recent goal slightly higher.
            var orderedGoals = goals
                .Where(goal => goal.DesiredEffects.Any(b => !b.Evaluate()))
                .OrderByDescending(goal => goal == mostRecentGoal ? goal.Priority - 0.01 : goal.Priority)
                .ToList();

            // Attempt to solve each goal in order of priority.
            foreach (var goal in orderedGoals)
            {
                var goalNode = new Node(null, null, goal.DesiredEffects, 0);

                // If a valid path to the goal is found, construct and return the plan.
                if (FindPath(goalNode, agent.actions))
                {
                    if (goalNode.IsLeafDead) continue; // Skip if no viable actions exist.

                    var actionStack = new Stack<AgentAction>();
                    while (goalNode.Leaves.Count > 0)
                    {
                        var cheapestLeaf = goalNode.Leaves.OrderBy(leaf => leaf.Cost).First();
                        goalNode = cheapestLeaf;
                        actionStack.Push(cheapestLeaf.Action);
                    }

                    return new ActionPlan(goal, actionStack, goalNode.Cost);
                }
            }
            return null;
        }

        /// <summary>
        ///     Recursively explores possible action sequences to fulfill required effects.
        /// </summary>
        /// <param name="parent">The current node in the action tree.</param>
        /// <param name="actions">A set of actions available to the agent.</param>
        /// <returns>True if a valid path is found; otherwise, false.</returns>
        private bool FindPath(Node parent, HashSet<AgentAction> actions)
        {
            var orderedActions = actions.OrderBy(a => a.Cost);
            foreach (var action in orderedActions)
            {
                var requiredEffects = parent.RequieredEffects;

                // Remove fulfilled effects.
                requiredEffects.RemoveWhere(b => b.Evaluate());

                // If all effects are fulfilled, we have a complete plan.
                if (requiredEffects.Count == 0) return true;

                // Check if the action can fulfill any required effects.
                if (action.Effects.Any(requiredEffects.Contains))
                {
                    var newRequiredEffects = new HashSet<AgentBelief>(requiredEffects);
                    newRequiredEffects.ExceptWith(action.Effects);
                    newRequiredEffects.UnionWith(action.Preconditions);

                    var newAvailableActions = new HashSet<AgentAction>(actions);
                    newAvailableActions.Remove(action);

                    var newNode = new Node(parent, action, newRequiredEffects, parent.Cost + action.Cost);

                    // Recursively explore the new node.
                    if (FindPath(newNode, newAvailableActions))
                    {
                        parent.Leaves.Add(newNode);
                        newRequiredEffects.ExceptWith(newNode.Action.Preconditions);
                    }

                    // Return true if all effects at this depth are satisfied.
                    if (newRequiredEffects.Count == 0) return true;
                }
            }

            return false;
        }
    }
}