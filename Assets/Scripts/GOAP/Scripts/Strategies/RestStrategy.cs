using GOAP.Scripts;
using GOAP.Scripts.AgentStats;
using UnityEngine;

public class RestStrategy : IActionStrategy
{
    StatSystem statSystem;
    private float duration;
    private float elapsed;
    private int increseAmount = 50; // Amount to reduce boredom by
    public bool Complete { get; private set; }
    public bool CanPerform => true;

    public RestStrategy(float duration, StatSystem statSystem)
    {
        this.duration = duration;
        this.statSystem = statSystem;
        elapsed = 0f;
        Complete = false;
    }

    public void Start()
    {
        elapsed = 0f;
        Complete = false;
        // Aquí se podría activar una animación de descanso.
        Debug.Log("Inicio del descanso");
        if (statSystem != null && statSystem.TryGetStat("Stamina", out var boredomStat))
        {
            boredomStat.Increase(increseAmount);
        }
    }

    public void Update(float deltaTime)
    {
        if (Complete) return;
        elapsed += deltaTime;
        if (elapsed >= duration)
        {
            Complete = true;
            Debug.Log("Descanso completado");
        }
    }

    public void Stop()
    {
        // Se puede reiniciar o limpiar estados si es necesario.
        Complete = false;
    }
}