
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Synchronizes sprite-based animations across different renderers and manages frame updates.
/// </summary>
public class AnimationSync
{
    private SpriteRenderer bodyRenderer;  
    private SpriteRenderer hairRenderer;  
    private SpriteRenderer toolRenderer;  
    private HairStyle hairType;  
    
    private Dictionary<AnimationAction, Sprite[]> bodyAnimations;
    private Dictionary<AnimationAction, Sprite[]> hairAnimations;
    private Dictionary<AnimationAction, Sprite[]> toolAnimations;
    
    private AnimationAction currentAnimation;
    
    private int currentFrame = 0;
    private float frameTime = 0f;

    /// <summary>
    /// Delay between frames in seconds.
    /// </summary>
    public float frameDelay = 0.1f;

    private AnimationLoader animationLoader;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnimationSync"/> class.
    /// </summary>
    /// <param name="bodyRenderer">The SpriteRenderer for the body.</param>
    /// <param name="hairRenderer">The SpriteRenderer for the hair.</param>
    /// <param name="toolRenderer">The SpriteRenderer for the tool.</param>
    /// <param name="hairType">The hair style to use.</param>
    /// <param name="animationLoader">The loader responsible for fetching animations.</param>
    public AnimationSync(SpriteRenderer bodyRenderer, SpriteRenderer hairRenderer, SpriteRenderer toolRenderer, HairStyle hairType)
    {
        this.bodyRenderer = bodyRenderer;
        this.hairRenderer = hairRenderer;
        this.toolRenderer = toolRenderer;
        this.hairType = hairType;
        this.animationLoader = new AnimationLoader();

        bodyAnimations = new Dictionary<AnimationAction, Sprite[]>();
        hairAnimations = new Dictionary<AnimationAction, Sprite[]>();
        toolAnimations = new Dictionary<AnimationAction, Sprite[]>();
        
        LoadAllAnimations();
    }
    
    /// <summary>
    /// Gets the current frame index of the animation.
    /// </summary>
    /// <returns>The current frame index.</returns>
    public int GetCurrentFrame() => currentFrame;

    /// <summary>
    /// Gets the total number of frames for the current animation.
    /// </summary>
    /// <returns>The total frame count, or 1 if not available.</returns>
    public int GetTotalFrames() => bodyAnimations.ContainsKey(currentAnimation) ? bodyAnimations[currentAnimation].Length : 1;

    /// <summary>
    /// Updates the animation based on the new action, if provided, and applies the correct sprites.
    /// </summary>
    /// <param name="newAction">
    /// The new animation action to switch to. If <c>null</c>, continues with the current animation.
    /// </param>
    public void UpdateAnim(AnimationAction? newAction)
    {
        // If a new action is provided and it's different from the current, reset the frame index.
        if (newAction.HasValue && newAction.Value != currentAnimation)
        {
            currentAnimation = newAction.Value;
            currentFrame = 0;
            frameTime = 0f;
        }

        if (!bodyAnimations.ContainsKey(currentAnimation)) return;
        UpdateAnimationFrame();
        bodyRenderer.sprite = bodyAnimations[currentAnimation][currentFrame];

        if (hairAnimations.ContainsKey(currentAnimation) && currentFrame < hairAnimations[currentAnimation].Length)
            hairRenderer.sprite = hairAnimations[currentAnimation][currentFrame];

        if (toolAnimations.ContainsKey(currentAnimation) && currentFrame < toolAnimations[currentAnimation].Length)
            toolRenderer.sprite = toolAnimations[currentAnimation][currentFrame];
    }

    /// <summary>
    /// Updates the facing direction of all sprite renderers based on horizontal movement.
    /// </summary>
    /// <param name="moveDirection">The horizontal movement direction (negative for left, positive for right).</param>
    public void UpdateFacingDirection(float moveDirection)
    {
        bool flip = moveDirection < 0;
        bodyRenderer.flipX = flip;
        hairRenderer.flipX = flip;
        toolRenderer.flipX = flip;
    }

    /// <summary>
    /// Advances the animation frame based on the elapsed time.
    /// </summary>
    void UpdateAnimationFrame()
    {
        frameTime += Time.deltaTime;
        if (!(frameTime >= frameDelay)) return;
        currentFrame = (currentFrame + 1) % bodyAnimations[currentAnimation].Length;
        frameTime = 0f;
    }

    /// <summary>
    /// Loads all animations for the body, hair, and tool using the provided AnimationLoader.
    /// </summary>
    void LoadAllAnimations()
    {
        string basePath = "Human";

        // Load body animations.
        if (bodyRenderer == null) return;
        bodyAnimations = animationLoader.LoadAnimations(basePath, "base");
        
        // Load hair animations.
        if (hairRenderer == null) return;
        hairAnimations = animationLoader.LoadAnimations(basePath, hairType.ToString());
        
        // Load tool animations.
        if (toolRenderer == null) return;
        toolAnimations = animationLoader.LoadAnimations(basePath, "tools");
    }
}
