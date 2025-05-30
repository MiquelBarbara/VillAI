using System;
using UnityEngine;

namespace GameplayFocused.TimeManagment
{
    /// <summary>
    /// A component that acts as an agent to be notified on time phase changes.
    /// </summary>
    public class TimeAgent : MonoBehaviour
    {
        /// <summary>
        /// Event invoked on each time tick as determined by the DayNightController.
        /// </summary>
        protected Action OnTimeTick;

        private void Start()
        {
            Initialize();
        }

        /// <summary>
        /// Subscribes this time agent to the GameManager's TimeController.
        /// </summary>
        private void Initialize()
        {
            GameManager.Instance.TimeController.Subscribe(this);
        }

        /// <summary>
        /// Invokes the onTimeTick action.
        /// </summary>
        public void Invoke()
        {
            OnTimeTick?.Invoke();
        }

        /// <summary>
        /// Unsubscribes this time agent from the TimeController when destroyed.
        /// </summary>
        protected virtual void OnDestroy()
        {
            GameManager.Instance.TimeController.Unsubscribe(this);
        }
    }
}