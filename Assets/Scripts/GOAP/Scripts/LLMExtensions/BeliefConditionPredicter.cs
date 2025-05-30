using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLM.Services;
using UnityServiceLocator;


public class BeliefConditionPredicter
{
    private List<AgentBelief> _beliefs = new List<AgentBelief>();
    private GoapAgent _agent;

    public BeliefConditionPredicter(GoapAgent agent)
    {
        _agent = agent;
    }

    public void RegisterBelief(AgentBelief belief)
    {
        _beliefs.Add(belief);
    }
    
    public IEnumerator ReevaluateBelief(GoapAgent agent)
    {
        var template = new GOAPBeliefConditionData
        {
            evaluatedBelief = _beliefs
        };
        
        ServiceLocator.Global.Get<IPredictionService>(out var service);
        //yield return service.Predict(template);
        
        var conditions = new List<bool>();

        for (int i = 0; i < _beliefs.Count; i++)
        {
            var belief = _beliefs[i];
            var i1 = i;
            _beliefs[i].NewCondition(() => conditions[i1]);
        }

        yield break;
    }
}
