using System;
using Blackboard_Architecture;
using EntitiesRelated.Interaction;
using GOAP.Scripts;
using UnityEngine.AI;

public class TalkStrategy : IActionStrategy
{
    private readonly NavMeshAgent agent;
    private readonly Func<Interactable> interactable;
    private readonly InteractController interactController;
    private float elapsedTime;
    private readonly float talkDuration = 3f; // Duración del diálogo

    public TalkStrategy(NavMeshAgent agent, InteractController interactController, Func<Interactable> interactable)
    {
        this.agent = agent;
        this.interactController = interactController;
        this.interactable = interactable;
    }

    public bool CanPerform => !Complete;
    public bool Complete => elapsedTime >= talkDuration;

    public void Start()
    {
        elapsedTime = 0f;
        agent.isStopped = true; 
        interactController.Interact(interactable());
    }

    public void Update(float deltaTime)
    {
        
    }

    public void Stop()
    {
        agent.isStopped = false;
    }
}