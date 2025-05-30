using System;

namespace EntitiesRelated.Animation.Events
{
    /// <summary>
    /// Represents an animation event that triggers a callback at a specific normalized time during playback.
    /// </summary>
    [Serializable]
    public class AnimationEventData
    {
        /// <summary>
        /// The normalized time (between 0 and 1) when this event should trigger.
        /// </summary>
        public float triggerNormalizedTime;
    
        /// <summary>
        /// The callback to invoke when the event triggers.
        /// </summary>
        public Action Callback;

        /// <summary>
        /// Indicates whether this event has already been fired.
        /// </summary>
        [NonSerialized]
        public bool Fired;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationEventData"/> class.
        /// </summary>
        /// <param name="triggerNormalizedTime">The normalized time at which to trigger the event.</param>
        /// <param name="callback">The callback to invoke when the event triggers.</param>
        public AnimationEventData(float triggerNormalizedTime, Action callback)
        {
            this.triggerNormalizedTime = triggerNormalizedTime;
            Callback = callback;
            Fired = false;
        }
    }
}