using System;
using EntitiesRelated.Core;
using GameplayFocused.ExtractingResource;
using UnityEngine;
using UnityEngine.AI;
using GOAP.Scripts;

public class ObtainItemStrategy : IActionStrategy
{
    private GameObject agent;
    private Func<GameObject> targetResource;
    private bool complete;
    private NavMeshAgent navMeshAgent;

    // Define a minimum distance for interaction.
    private float interactionDistance = 2f;

    private enum State { Moving, Interacting, Completed }
    private State currentState = State.Moving;

    public ObtainItemStrategy(GameObject agent, Func<GameObject> targetResource)
    {
        this.agent = agent;
        this.targetResource = targetResource;
        navMeshAgent = agent.GetComponent<NavMeshAgent>();
        complete = false;
    }

    public bool Complete => complete;
    
    public bool CanPerform => agent != null && targetResource != null;
    
    public void Start()
    {
        if (navMeshAgent != null)
        {
            // Command the agent to move to the resource.
            navMeshAgent.SetDestination(targetResource().transform.position);
        }
        currentState = State.Moving;
    }
    
    public void Update(float deltaTime)
    {
        if (currentState == State.Moving)
        {
            // Check if the agent is close enough to interact.
            if (Vector3.Distance(agent.transform.position, targetResource().transform.position) <= interactionDistance)
            {
                // Stop the agent's movement.
                if (navMeshAgent != null)
                {
                    navMeshAgent.ResetPath();
                }
                currentState = State.Interacting;
            }
        }
        if (currentState == State.Interacting)
        {
            // Once in range, interact with the resource.
            GatherableResource resource = targetResource().GetComponent<GatherableResource>();
            if (resource != null)
            {
                resource.Interact(agent.GetComponent<Character>());
            }
            currentState = State.Completed;
            complete = true;
        }
    }
    
    public void Stop()
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.ResetPath();
        }
    }
}
