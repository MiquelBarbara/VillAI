using Blackboard_Architecture;
using GOAP.Scripts;
using UnityEngine;

public class CompleteQuestStrategy : IActionStrategy
{
    private readonly GoapAgent agent;
    private readonly Blackboard blackboard;

    private bool done;

    public CompleteQuestStrategy(GoapAgent agent, Blackboard blackboard)
    {
        this.agent = agent;
        this.blackboard = blackboard;
    }

    public bool CanPerform { get; }
    public bool Complete { get; }

    public void Start()
    {
        // Example: verify that the agent has the required items, or is at the quest giver, etc.
        // If everything is correct, we mark the quest as complete
        done = true;
        Debug.Log($"{agent.name} completed the quest!");
    }

    public void Update(float deltaTime)
    {
        // Not needed if it completes instantly
    }

    public void Stop() { }

    public bool IsComplete => done;
    public bool IsCanceled => false;
}