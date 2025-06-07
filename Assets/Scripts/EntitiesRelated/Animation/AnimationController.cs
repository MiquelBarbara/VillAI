using System;
using System.Collections.Generic;
using System.Linq;
using EntitiesRelated;
using EntitiesRelated.Animation;
using EntitiesRelated.Animation.Commands;
using EntitiesRelated.Animation.Events;
using UnityEngine;

/// <summary>
/// Controls animation playback, synchronization, and event triggering for character sprites.
/// </summary>
public class AnimationController : MonoBehaviour, IAnimationCommandReceiver 
{
    [SerializeField] private Animator animator;
    [SerializeField] private float defaultTransitionDuration = 0.1f;
    
    [SerializeField] private SpriteRenderer bodyRenderer;  
    [SerializeField] private SpriteRenderer hairRenderer;  
    [SerializeField] private SpriteRenderer toolRenderer;  
    [SerializeField] private HairStyle hairType;  
    
    private AnimationSync animationSync;
    private AnimationCommand currentAnimation;
    private AnimationEventHandler eventHandler = new AnimationEventHandler();
    
    public void Start() {
        animationSync = new AnimationSync(bodyRenderer, hairRenderer, toolRenderer, hairType);
    }

    /// <summary>
    /// Checks if the current animation is playing and not idle or walking.
    /// </summary>
    /// <returns> True if an animation is playing, false otherwise.</returns>
    public bool IsPlayingAnimation()
    {
        if(currentAnimation == null)
            return false;
        return currentAnimation.Action != AnimationAction.idle && 
               currentAnimation.Action != AnimationAction.walk;
    }

    /// <summary>
    /// Receives an animation command and plays the corresponding animation.
    /// </summary>
    /// <param name="command"> The animation command to execute.</param>
    /// <param name="transitionDuration"> The duration of the transition to the new animation. If negative, uses the default transition duration.</param>
    public void ReceiveAnimationCommand(AnimationCommand command, float transitionDuration = -1f)
    {
        if (currentAnimation == command)
            return;
        currentAnimation = command;
        eventHandler.SetEvents(command.Events);
        float transDuration = transitionDuration < 0 ? defaultTransitionDuration : transitionDuration;
        animator.CrossFade(command.Action.ToString(), transDuration);
    }

    /// <summary>
    /// Updates the facing direction of the character based on the provided direction.
    /// </summary>
    /// <param name="direction"> The horizontal movement direction (negative for left, positive for right).</param>
    public void UpdateFacing(float direction)
    {
        animationSync.UpdateFacingDirection(direction);
    }
    
    void Update() {
        if(currentAnimation == null) return;
        animationSync.UpdateAnim(currentAnimation.Action);
        eventHandler.UpdateEvents(animationSync.GetCurrentFrame(), animationSync.GetTotalFrames());
    }
    
}
