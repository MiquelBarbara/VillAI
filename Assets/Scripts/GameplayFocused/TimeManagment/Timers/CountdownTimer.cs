/// <summary>
/// A timer that counts down from a specified value.
/// </summary>
public class CountdownTimer : Timer
{
    /// <summary>
    /// Initializes a new instance of the CountdownTimer class.
    /// </summary>
    /// <param name="value">The countdown starting value.</param>
    public CountdownTimer(float value) : base(value) { }

    /// <summary>
    /// Gets a value indicating whether the timer has finished.
    /// </summary>
    public bool IsFinished => Time <= 0;

    /// <summary>
    /// Decrements the timer by deltaTime.
    /// Stops the timer when it reaches zero.
    /// </summary>
    /// <param name="deltaTime">The time elapsed since the last tick.</param>
    public override void Tick(float deltaTime)
    {
        if (IsRunning && Time > 0)
            Time -= deltaTime;

        if (IsRunning && Time <= 0)
            Stop();
    }

    /// <summary>
    /// Resets the timer to the initial value.
    /// </summary>
    public void Reset()
    {
        Time = InitialTime;
    }

    /// <summary>
    /// Resets the timer to a new specified value.
    /// </summary>
    /// <param name="newTime">The new time value to reset to.</param>
    public void Reset(float newTime)
    {
        InitialTime = newTime;
        Reset();
    }
}