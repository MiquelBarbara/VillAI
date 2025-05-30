using Blackboard_Architecture;

namespace GOAP.Scripts
{
    public class CraftStrategy: IActionStrategy
    {
        
        private readonly GoapAgent agent;
        private readonly Blackboard blackboard;
        
        public CraftStrategy(GoapAgent agent, Blackboard blackboard)
        {
            this.agent = agent;
            this.blackboard = blackboard;
        }
        public bool CanPerform { get; }
        public bool Complete { get; }
    }
}