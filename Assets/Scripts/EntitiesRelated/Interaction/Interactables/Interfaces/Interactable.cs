using EntitiesRelated.Animation.Commands;
using EntitiesRelated.Animation.Events;
using EntitiesRelated.Core;
using UnityEngine;

/// <summary>
/// Represents an interactable object in the game world.
/// </summary>
public interface Interactable
{
    /// <summary>
    /// Defines the interaction behavior for an object when a character interacts with it.
    /// </summary>
    void Interact(Character character);
    
    Transform GetTransform();

    /// <summary>
    /// Returns the animation action associated with this interaction.
    /// </summary>
    AnimationCommand GetInteractionAnimation();
}