namespace EntitiesRelated.Animation.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EntitiesRelated.Animation.Events;

    /// <summary>
    /// Handles animation events by managing a list of active events and triggering callbacks at specified normalized times.
    /// </summary>
    public class AnimationEventHandler {
        private List<AnimationEventData> activeEvents = new List<AnimationEventData>();
        private bool animationFinishedFired = false;

        public void SetEvents(List<AnimationEventData> events) {
            activeEvents.Clear();
            if (events != null) {
                foreach (var e in events) {
                    activeEvents.Add(new AnimationEventData(e.triggerNormalizedTime, e.Callback));
                }
            }
            animationFinishedFired = false;
        }

        /// <summary>
        /// Updates the state of animation events based on the current frame and total frames.
        /// </summary>
        /// <param name="currentFrame"> The current frame of the animation.</param>
        /// <param name="totalFrames"> The total number of frames in the animation.</param>
        public void UpdateEvents(int currentFrame, int totalFrames) {
            if (totalFrames == 0)
                return;

            float normalizedTime = (float)currentFrame / (totalFrames - 1);
            foreach (var ev in activeEvents.Where(e => !e.Fired && normalizedTime >= e.triggerNormalizedTime)) {
                ev.Callback?.Invoke();
                ev.Fired = true;
            }
            bool isAnimationFinished = normalizedTime >= 1f;
            if (!isAnimationFinished || animationFinishedFired) return;
            animationFinishedFired = true;
        }
    }

}