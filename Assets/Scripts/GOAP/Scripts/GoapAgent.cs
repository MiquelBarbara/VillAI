
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blackboard_Architecture;
using Game.Utilities.DependencyInjection;
using GOAP.ScriptableObjects;
using GOAP.Scripts;
using GOAP.Scripts.AgentStats;
using GOAP.Scripts.Configuration.Binding;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody2D))]
public class GoapAgent : MonoBehaviour
{
    /// <summary>
    /// The NavMeshAgent component used for navigation.
    /// </summary>
    public NavMeshAgent navMeshAgent;

    [SerializeField] private StatSystem _statSystem;
    public BlackboardController BlackboardController;
    public bool isStopped;
    public ActionPlan actionPlan;
    public HashSet<AgentAction> actions;
    public BeliefFactory beliefFactory;
    public Dictionary<string, AgentBelief> beliefs;
    public AgentAction currentAction;
    public AgentGoal currentGoal;
    private Vector3 destination;
    public HashSet<AgentGoal> goals;
    private IGoapPlanner gPlanner;
    private AIGoapPlanner aIGoapPlanner;
    private AgentGoal lastGoal;
    private Rigidbody2D rb;
    private GameObject target;
    public HashSet<ISensor> sensors;

    [Header("Capabilities")]
    [SerializeField] private List<CapabilitySO> capabilitySOs;

    [SerializeField] private bool poweredByAI = false;
    [SerializeField] private GoapFactory gFactory;
    
    public InGameCountdownTimer planCountdownTimer;
    public InGameCountdownTimer goalCountdownTimer;
    public InGameCountdownTimer beliefsCountdownTimer;
    public InGameCountdownTimer inventoryCountdownTimer;
    
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
         BlackboardController = GetComponent<BlackboardController>();
        _statSystem = GetComponent<StatSystem>();
        
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        gPlanner = gFactory.CreatePlanner();
        aIGoapPlanner = new AIGoapPlanner();

        isStopped = false;

        if (!poweredByAI) return;
        planCountdownTimer = GetInGameCountdownTimer(50, 110, ()=>aIGoapPlanner.PredictPlan(this, goals));
        goalCountdownTimer = GetInGameCountdownTimer(40, 60, ()=>aIGoapPlanner.CheckGoalsPriority(this, goals));
        beliefsCountdownTimer = GetInGameCountdownTimer(1, 20, () => aIGoapPlanner.CheckBeliefs(this, beliefs));
    }

    private InGameCountdownTimer GetInGameCountdownTimer(float maxTime, float minTime, Func<IEnumerator> routine)
    {
        var timer = new InGameCountdownTimer(Random.Range(minTime, maxTime));
        timer.Start();
        timer.OnTimerStop += () =>
        {
            timer.Reset(Random.Range(minTime, maxTime));
            StartCoroutine(routine());
            timer.Start();
        };
        return timer;
    }

    private void Start()
    {
        beliefs = new Dictionary<string, AgentBelief>();
        beliefFactory = new BeliefFactory(this, beliefs);
    
        actions = new HashSet<AgentAction>();
        goals = new HashSet<AgentGoal>();
        sensors = new HashSet<ISensor>();
        
        foreach (var config in capabilitySOs)
        {
            config.GetConfig().Configure(this);
        }
    }

    /// <summary>
    /// Registers a sensor with the agent.
    /// </summary>
    /// <param name="sensor">The sensor to register.</param>
    public void RegisterSensor(ISensor sensor)
    {
        sensors.Add(sensor);
    }
    
    private void UpdatePlan()
    {
        if (currentAction != null) return;
        CalculatePlan();

        if (actionPlan == null || actionPlan.Actions.Count <= 0) return;
        currentGoal = actionPlan.AgentGoal;
        currentAction = actionPlan.Actions.Pop();

        if (currentAction.Preconditions.All(b => b.Evaluate()))
        {
            currentAction.Start();
        }
        else
        {
            currentAction = null;
            currentGoal = null;
        }
    }

    private void Update()
    {
        if (poweredByAI)
        {
            planCountdownTimer.Tick(Time.deltaTime);
            goalCountdownTimer.Tick(Time.deltaTime);
            beliefsCountdownTimer.Tick(Time.deltaTime);
        }
        
        foreach (var stat in _statSystem.GetAllStats())
        {
            stat.Update(Time.deltaTime);
        }
        
        foreach (var sensor in sensors)
        {
            sensor.UpdateSensor();
        }

        if (isStopped)
            return;
        
        UpdatePlan();
        
        if (actionPlan == null || currentAction == null) return;
        currentAction.Update(Time.deltaTime);

        if (!currentAction.Complete) return;
        currentAction.Stop();
        currentAction = null;

        if (actionPlan.Actions.Count != 0) return;
        lastGoal = currentGoal;
        currentGoal = null;
    }

    /// <summary>
    /// Stops the agent's actions.
    /// </summary>
    public void Stop()
    {
        isStopped = true;
    }

    /// <summary>
    /// Resumes the agent's actions.
    /// </summary>
    public void Resume()
    {
        isStopped = false;
    }
    
    /// <summary>
    /// Calculates a new action plan based on the agent's current goals and priorities.
    /// Considers only goals with higher priority than the current goal if one exists.
    /// </summary>
    private void CalculatePlan()
    {
        var priorityLevel = currentGoal?.Priority ?? 0;
        var goalsToCheck = goals;

        if (currentGoal != null)
        {
            goalsToCheck = new HashSet<AgentGoal>(goals.Where(g => g.Priority > priorityLevel));
        }

        var potentialPlan = poweredByAI ? aIGoapPlanner.Plan(this, goalsToCheck) : gPlanner.Plan(this, goalsToCheck, lastGoal);
        
        if (potentialPlan != null)
            actionPlan = potentialPlan;
    }
    
}
