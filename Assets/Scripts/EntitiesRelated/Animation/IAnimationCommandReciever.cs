using EntitiesRelated.Animation.Commands;

namespace EntitiesRelated.Animation
{
    public interface IAnimationCommandReceiver
    {
        bool IsPlayingAnimation();
        void ReceiveAnimationCommand(AnimationCommand command, float duration = -1);
        void UpdateFacing(float direction);
    }
}