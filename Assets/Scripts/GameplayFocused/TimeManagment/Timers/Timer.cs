using System;
using UnityEngine;

/// <summary>
/// Abstract base class for timers, providing common timer functionality.
/// </summary>
public abstract class Timer
{
    /// <summary>
    /// The initial time value for the timer.
    /// </summary>
    protected float InitialTime;

    /// <summary>
    /// Event invoked when the timer starts.
    /// </summary>
    public Action OnTimerStart = delegate { };

    /// <summary>
    /// Event invoked when the timer stops.
    /// </summary>
    public Action OnTimerStop = delegate { };

    /// <summary>
    /// Initializes a new instance of the Timer class.
    /// </summary>
    /// <param name="value">The initial time value.</param>
    protected Timer(float value)
    {
        InitialTime = value;
        IsRunning = false;
    }

    /// <summary>
    /// Gets or sets the current time remaining.
    /// </summary>
    protected float Time { get; set; }

    /// <summary>
    /// Gets a value indicating whether the timer is running.
    /// </summary>
    protected bool IsRunning { get; set; }

    /// <summary>
    /// Gets the progress of the timer as a fraction of the initial time.
    /// </summary>
    public float Progress => Time / InitialTime;

    /// <summary>
    /// Starts the timer and resets it to the initial time.
    /// </summary>
    public void Start()
    {
        Time = InitialTime;
        if (IsRunning) return;
        IsRunning = true;
        OnTimerStart.Invoke();
    }

    /// <summary>
    /// Stops the timer.
    /// </summary>
    public void Stop()
    {
        if (!IsRunning) return;
        IsRunning = false;
        OnTimerStop.Invoke();
    }

    /// <summary>
    /// Resumes the timer.
    /// </summary>
    public void Resume()
    {
        IsRunning = true;
    }

    /// <summary>
    /// Pauses the timer.
    /// </summary>
    public void Pause()
    {
        IsRunning = false;
    }

    /// <summary>
    /// Updates the timer by reducing or increasing the time based on deltaTime.
    /// </summary>
    /// <param name="deltaTime">The time elapsed since the last update.</param>
    public abstract void Tick(float deltaTime);
}