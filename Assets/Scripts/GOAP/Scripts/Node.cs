using System.Collections.Generic;

namespace GOAP.Scripts
{
    /// <summary>
    ///     Represents a node in the GOAP planning graph. Each node corresponds to a state in the planning process,
    ///     connecting actions and their required effects.
    /// </summary>
    public class Node
    {
        /// <summary>
        ///     Constructs a new node in the planning graph.
        /// </summary>
        /// <param name="parent">The parent node leading to this node.</param>
        /// <param name="action">The action that transitions from the parent node to this node.</param>
        /// <param name="effects">The set of required effects for this node.</param>
        /// <param name="cost">The cumulative cost to reach this node.</param>
        public Node(Node parent, AgentAction action, HashSet<AgentBelief> effects, float cost)
        {
            Parent = parent;
            Action = action;
            RequieredEffects = new HashSet<AgentBelief>(effects);
            Leaves = new List<Node>();
            Cost = cost;
        }

        /// <summary>
        ///     The parent node from which this node was generated.
        /// </summary>
        public Node Parent { get; }

        /// <summary>
        ///     The action associated with this node that transitions the parent state to this state.
        /// </summary>
        public AgentAction Action { get; }

        /// <summary>
        ///     The set of beliefs (effects) required to fulfill the current node's goal.
        /// </summary>
        public HashSet<AgentBelief> RequieredEffects { get; }

        /// <summary>
        ///     The list of child nodes branching from this node.
        /// </summary>
        public List<Node> Leaves { get; }

        /// <summary>
        ///     The cumulative cost to reach this node in the planning graph.
        /// </summary>
        public float Cost { get; }

        /// <summary>
        ///     Determines whether this node is a "dead leaf," meaning it has no child nodes and no associated action.
        /// </summary>
        public bool IsLeafDead => Leaves.Count == 0 && Action == null;
    }
}