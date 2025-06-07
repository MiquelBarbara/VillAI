using EntitiesRelated.Animation.Commands;

namespace EntitiesRelated.Animation
{
    /// <summary>
    /// Interface for receiving and processing animation commands.
    /// </summary>
    public interface IAnimationCommandReceiver
    {
        bool IsPlayingAnimation();
        void ReceiveAnimationCommand(AnimationCommand command, float duration = -1);
        void UpdateFacing(float direction);
    }
}