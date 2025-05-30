using System; using System.Collections.Generic; using EntitiesRelated.Animation.Events;


namespace EntitiesRelated.Animation.Commands
{
    public abstract class AnimationCommand
    {
        public AnimationAction Action { get; }
        public List<AnimationEventData> Events { get; }

        protected AnimationCommand(AnimationAction action, List<AnimationEventData> events = null)
        {
            Action = action;
            Events = events ?? new List<AnimationEventData>();
        }
    }

    public class IdleAnimationCommand : AnimationCommand
    {
        public IdleAnimationCommand(List<AnimationEventData> events = null) : base(AnimationAction.idle, events)
        {
        }
    }

    public class WalkAnimationCommand : AnimationCommand
    {
        public WalkAnimationCommand(List<AnimationEventData> events = null) : base(AnimationAction.walk, events)
        {
        }
    }

    public class MiningAnimationCommand : AnimationCommand
    {
        public MiningAnimationCommand(List<AnimationEventData> events = null) : base(AnimationAction.mining, events)
        {
        }
    }

    public class DoingAnimationCommand : AnimationCommand
    {
        public DoingAnimationCommand(List<AnimationEventData> events = null) : base(AnimationAction.doing, events)
        {
        }
    }

    public class DigAnimationCommand : AnimationCommand
    {
        public DigAnimationCommand(List<AnimationEventData> events = null) : base(AnimationAction.dig, events)
        {
        }
    }

    public class WateringAnimationCommand : AnimationCommand
    {
        public WateringAnimationCommand(List<AnimationEventData> events = null) : base(AnimationAction.watering, events)
        {
        }
    }

    public class AxeAnimationCommand : AnimationCommand
    {
        public AxeAnimationCommand(List<AnimationEventData> events = null) : base(AnimationAction.axe, events)
        {
        }
    }
}






