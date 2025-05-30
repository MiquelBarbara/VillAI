using System.Collections.Generic;
using GOAP.Scripts;
using UnityEngine;

namespace LLM.Templates
{
    public class GoapGoalPriorityData : DataTransferObject
    {
        [Input]
        public List<AgentGoal> objectives;
        
        [Output]
        public List<int> priorities;
    }
}