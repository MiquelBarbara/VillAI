using System;
using EntitiesRelated.Animation.Commands;
using EntitiesRelated.Animation.Events;
using EntitiesRelated.Core;
using GameplayFocused.ExtractingResource.CollectStrategy;
using UnityEngine;

namespace GameplayFocused.ExtractingResource.Implementations
{
    /// <summary>
    /// Represents an animal resource that can be gathered from an animal.
    /// </summary>
    public class AnimalResource : GatherableResource
    {
        /// <summary>
        /// Initializes the AnimalResource component.
        /// Sets up the cooldown timer and assigns a default collection strategy (InventoryCollectionStrategy).
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            // Assign a default collection strategy if none is set.
            collectionStrategy ??= new InventoryCollectionStrategy();
            _cooldownTimer.OnTimerStop += () =>
            {
                Gathered = true;
                Register<AnimalResource>();
            };
            
            _cooldownTimer.OnTimerStart += () =>
            {
                Gathered = false;
                Unregister<AnimalResource>();
            };
        }
        
        public override void Register<T>()
        {
            ActiveObjectRegistry<AnimalResource>.Register(this);
        }
        
        public override void Unregister<T>()
        {
            ActiveObjectRegistry<AnimalResource>.Unregister(this);
        }
        
        /// <summary>
        /// Processes interaction with the animal resource.
        /// If the resource is in cooldown, logs a message and does nothing.
        /// Otherwise, gathers the resource and restarts the cooldown.
        /// </summary>
        /// <param name="interactor">The character interacting with the resource.</param>
        public override void Interact(Character interactor)
        {
            if(!_cooldownTimer.IsFinished)
            {
                return;
            }
            // Gather the resource and start the cooldown timer.
            Gather(interactor);
            _cooldownTimer.Start();
        }
    
        /// <summary>
        /// Gets the interaction animation for the animal resource.
        /// </summary>
        /// <returns>
        /// A tuple containing the <see cref="AnimationAction"/> (doing) and an empty array of <see cref="AnimationEventData"/>.
        /// </returns>
        public override AnimationCommand GetInteractionAnimation()
        {
            return new DoingAnimationCommand();
        }
    }
}
