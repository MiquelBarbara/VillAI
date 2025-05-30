using GameplayFocused.TimeManagment;

/// <summary>
/// A countdown timer that converts real deltaTime to in-game minutes based on the DayNightController's time scale.
/// </summary>
public class InGameCountdownTimer : CountdownTimer
{
    /// <summary>
    /// Initializes a new instance of the InGameCountdownTimer class.
    /// </summary>
    /// <param name="inGameMinute">The in-game minute value to count down from.</param>
    public InGameCountdownTimer(float inGameMinute) : base(inGameMinute) { }
    
    /// <summary>
    /// Decrements the timer by converting real deltaTime into in-game minutes.
    /// </summary>
    /// <param name="deltaTime">The real time elapsed since the last tick.</param>
    public override void Tick(float deltaTime)
    {
        // Convert deltaTime to in-game minutes.
        float gameDeltaMinutes = (deltaTime * DayNightController.Instance.GetTimeScale()) / 60f;
        base.Tick(gameDeltaMinutes);
    }
    
}