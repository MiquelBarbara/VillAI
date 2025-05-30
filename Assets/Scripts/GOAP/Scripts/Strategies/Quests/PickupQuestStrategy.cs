using Blackboard_Architecture;
using GOAP.Scripts;
using UnityEngine;

public class PickUpQuestStrategy : IActionStrategy
{
    private readonly GoapAgent agent;
    private readonly Blackboard blackboard;
    public PickUpQuestStrategy(GoapAgent agent, Blackboard blackboard)
    {
        this.agent = agent;
        this.blackboard = blackboard;
    }

    public bool CanPerform { get; }
    public bool Complete { get; }

    public void Start() 
    {
 
    }

    public void Update(float deltaTime)
    {
        // Usually picking up is instant, so we might mark the action as complete right away
    }

    public void Stop() { }

    public bool IsComplete => true;
    public bool IsCanceled => false;
}
