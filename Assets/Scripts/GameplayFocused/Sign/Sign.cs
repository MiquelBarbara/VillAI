using System;
using EntitiesRelated.Animation.Events;
using EntitiesRelated.Core;
using System.Collections.Generic;
using EntitiesRelated.Animation.Commands;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Represents an in-game sign that displays dialogue when interacted with.
    /// The sign can display dialogue in either a panel or bubble format.
    /// </summary>
    public abstract class Sign : MonoBehaviour, Interactable
    {
        /// <summary>
        /// Contains the dialogue lines and character information for the sign.
        /// </summary>
        [SerializeField] protected DialogueData dialogueData;
        
        /// <summary>
        /// Initiates the dialogue when the sign is interacted with.
        /// </summary>
        /// <param name="character">The character interacting with the sign.</param>
        public abstract void Interact(Character character);
        
        /// <summary>
        /// Gets the transform of the sign.
        /// </summary>
        /// <returns>The transform component of the sign GameObject.</returns>
        public Transform GetTransform()
        {
            return gameObject.transform;
        }

        /// <summary>
        /// Returns the interaction animation for the sign.
        /// </summary>
        /// <returns>
        /// A tuple containing the idle animation and a null array for animation events.
        /// </returns>
        public AnimationCommand GetInteractionAnimation()
        {
            return new IdleAnimationCommand();
        }
    }
}
