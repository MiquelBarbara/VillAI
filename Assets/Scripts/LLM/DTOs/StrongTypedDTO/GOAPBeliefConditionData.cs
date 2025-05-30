using System.Collections.Generic;
using GOAP;
using LLM.Templates;
using UnityEngine;

public class GOAPBeliefConditionData : DataTransferObject
{
    [Input] public List<AgentBelief> evaluatedBelief;
    
    
    [Output] public List<bool> conditions;
}