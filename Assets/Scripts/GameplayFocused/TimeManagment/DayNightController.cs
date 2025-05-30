using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace GameplayFocused.TimeManagment
{
    /// <summary>
    /// Controls the day/night cycle in the game.
    /// Updates in-game time, adjusts lighting based on an animation curve, and notifies subscribed time agents.
    /// </summary>
    public class DayNightController : Singleton<DayNightController>
    {
        private const float SecondsInDay = 86400f;  // Total seconds in a day (24 * 3600)
        private const float PhaseLength = 60f;       // Duration (in seconds) for a time phase tick
        
        /// <summary>
        /// The scale factor applied to real time to compute in-game time.
        /// </summary>
        public const float TimeScale = 72f; // Calculated as 86400 / 1200
        
        [SerializeField] private Color nightLightColor;
        [SerializeField] private AnimationCurve nightTimeCurve;
        [SerializeField] private Color dayLightColor = Color.white;
        [SerializeField] private float startTime;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Light2D light2D;

        private int _days;
        private float _time;

        /// <summary>
        /// Gets the current in-game hours.
        /// </summary>
        private float Hours => _time / 3600f;

        /// <summary>
        /// Gets the current in-game minutes.
        /// </summary>
        private float Minutes => _time % 3600f / 60f;

        private readonly List<TimeAgent> _timeAgents = new List<TimeAgent>();

        /// <summary>
        /// Gets the time scale factor used for converting real time to in-game time.
        /// </summary>
        /// <returns>The time scale value.</returns>
        public float GetTimeScale() => TimeScale;

        private void Start()
        {
            _time = startTime;
        }

        private void Update()
        {
            _time += Time.deltaTime * TimeScale;
            TimeValueCalculation();
            DayNight();
            if (_time > SecondsInDay)
            {
                NextDay();
            }
            TimeAgents();
        }

        private int oldPhase = 0;
        /// <summary>
        /// Checks for changes in time phases and invokes subscribed time agents when a new phase begins.
        /// </summary>
        private void TimeAgents()
        {
            int phase = (int)(_time / PhaseLength);
            if (phase == oldPhase) return;
            foreach (var timeAgent in _timeAgents)
            {
                timeAgent.Invoke();
            }
            oldPhase = phase;
        }

        /// <summary>
        /// Subscribes a time agent to be notified on time phase changes.
        /// </summary>
        /// <param name="timeAgent">The time agent to subscribe.</param>
        public void Subscribe(TimeAgent timeAgent)
        {
            _timeAgents.Add(timeAgent);
        }
    
        /// <summary>
        /// Unsubscribes a time agent.
        /// </summary>
        /// <param name="timeAgent">The time agent to remove.</param>
        public void Unsubscribe(TimeAgent timeAgent)
        {
            _timeAgents.Remove(timeAgent);
        }

        /// <summary>
        /// Returns the current in-game hours.
        /// </summary>
        /// <returns>Hours as a float.</returns>
        public float GetHours() => Hours;

        /// <summary>
        /// Returns the current in-game minutes.
        /// </summary>
        /// <returns>Minutes as a float.</returns>
        public float GetMinutes() => Minutes;
    
        /// <summary>
        /// Returns the total in-game time elapsed.
        /// </summary>
        /// <returns>The in-game time in seconds.</returns>
        public float GetTime() => _time;

        /// <summary>
        /// Adjusts the scene's lighting based on the time of day.
        /// Uses an animation curve to interpolate between day and night colors.
        /// </summary>
        private void DayNight()
        {
            var value = nightTimeCurve.Evaluate(Hours);
            var color = Color.Lerp(dayLightColor, nightLightColor, value);
            light2D.color = color;
        }

        /// <summary>
        /// Updates the displayed time in HH:MM format.
        /// </summary>
        private void TimeValueCalculation()
        {
            text.text = GetTimeText();
        }

        public string GetTimeText()
        {
            int hours = (int)Hours;
            int minutes = (int)Minutes;
            return hours.ToString("00") + ":" + minutes.ToString("00");
        }

        /// <summary>
        /// Advances to the next day by resetting the time and incrementing the day counter.
        /// </summary>
        private void NextDay()
        {
            _time = 0;
            _days += 1;
        }
    }
}
