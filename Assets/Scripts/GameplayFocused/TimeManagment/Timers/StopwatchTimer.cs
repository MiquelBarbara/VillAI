/// <summary>
/// A timer that counts up, functioning as a stopwatch.
/// </summary>
public class StopwatchTimer : Timer
{
    /// <summary>
    /// Initializes a new instance of the StopwatchTimer class.
    /// </summary>
    public StopwatchTimer() : base(0) { }

    /// <summary>
    /// Increments the timer by deltaTime.
    /// </summary>
    /// <param name="deltaTime">The time elapsed since the last tick.</param>
    public override void Tick(float deltaTime)
    {
        if (IsRunning)
            Time += deltaTime;
    }

    /// <summary>
    /// Resets the stopwatch to zero.
    /// </summary>
    public void Reset()
    {
        Time = 0;
    }

    /// <summary>
    /// Gets the current time of the stopwatch.
    /// </summary>
    /// <returns>The time elapsed.</returns>
    public float GetTime()
    {
        return Time;
    }
}