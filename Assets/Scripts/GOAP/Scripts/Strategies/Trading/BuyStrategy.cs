using GOAP.Scripts;
using UnityEngine;

public class BuyStrategy : IActionStrategy
{
    private float duration;
    private float timer;

    public BuyStrategy(float duration)
    {
        this.duration = duration;
        timer = 0f;
    }

    // Update is called by the action every frame with deltaTime.
    void Update(float deltaTime)
    {
        
    }

    public void Reset()
    {
        timer = 0f;
    }

    public bool CanPerform { get; }
    public bool Complete { get; }
}