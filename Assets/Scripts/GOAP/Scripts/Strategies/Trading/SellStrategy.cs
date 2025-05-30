using GOAP.Scripts;
using UnityEngine;

public class SellStrategy : IActionStrategy
{
    private float duration;
    private float timer;

    public SellStrategy(float duration)
    {
        this.duration = duration;
        timer = 0f;
    }

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