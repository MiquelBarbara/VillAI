using System.Collections.Generic;

namespace GOAP.Scripts
{
    /// <summary>
    /// Represents a plan of actions to achieve a specific goal in the GOAP system.
    /// Contains the goal, the sequence of actions to execute, and the total cost of the plan.
    /// </summary>
    public class ActionPlan
    {
        /// <summary>
        /// Constructs an ActionPlan instance with the specified goal, action sequence, and total cost.
        /// </summary>
        /// <param name="goal">The goal this plan aims to achieve.</param>
        /// <param name="actions">The stack of actions to execute for achieving the goal.</param>
        /// <param name="totalCost">The total cost of executing the plan.</param>
        public ActionPlan(AgentGoal goal, Stack<AgentAction> actions, float totalCost)
        {
            AgentGoal = goal;
            Actions = actions;
            TotalCost = totalCost;
        }

        /// <summary>
        /// The goal associated with this plan.
        /// </summary>
        public AgentGoal AgentGoal { get; }

        /// <summary>
        /// The sequence of actions to be executed as part of the plan.
        /// Actions are stored in a stack, where the top action is the next to be executed.
        /// </summary>
        public Stack<AgentAction> Actions { get; }

        /// <summary>
        /// The total cost of the plan, calculated based on the individual action costs.
        /// </summary>
        public float TotalCost { get; set; }
    }
}