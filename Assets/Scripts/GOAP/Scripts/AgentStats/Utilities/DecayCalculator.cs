using GameplayFocused.TimeManagment;
using UnityEngine;

public static class DecayCalculator
{
    /// <summary>
    /// Calculates the exponential decay constant k (per second) such that:
    /// targetValue = initialValue * exp(-k * totalTime)
    /// </summary>
    /// <param name="initialValue">Initial value at time 0 (must be > 0)</param>
    /// <param name="targetValue">Target value after the decay period (must be > 0 and less than initialValue for decay)</param>
    /// <param name="durationInGameMinutes">Duration over which decay happens, in in-game minutes</param>
    /// <returns>The decay constant k (per second)</returns>
    public static float CalculateExponentialDecayConstant(float initialValue, float targetValue, float durationInGameMinutes)
    {
        if (initialValue <= 0)
        {
            Debug.LogError("Initial value must be greater than zero.");
            return 0f;
        }
        
        if (targetValue <= 0)
        {
            Debug.LogError("Target value must be greater than zero.");
            return 0f;
        }

        // Convert minutes to seconds.
        float durationInSeconds = durationInGameMinutes * DayNightController.TimeScale;
        // k = - (1 / t) * ln(targetValue / initialValue)
        float k = -Mathf.Log(targetValue / initialValue) / durationInSeconds;
        return k;
    }

    /// <summary>
    /// Calculates the linear decay rate d (per second) such that:
    /// targetValue = initialValue - d * totalTime
    /// </summary>
    /// <param name="initialValue">Initial value at time 0</param>
    /// <param name="targetValue">Target value after the decay period</param>
    /// <param name="durationInGameMinutes">Duration over which decay happens, in in-game minutes</param>
    /// <returns>The linear decay rate d (per second)</returns>
    public static float CalculateLinearDecayRate(float initialValue, float targetValue, float durationInGameMinutes)
    {
        float durationInSeconds = durationInGameMinutes * 60f;
        float decayRate = (initialValue - targetValue) / durationInSeconds;
        return decayRate;
    }
}
