using System;
using System.Collections.Generic;
using EntitiesRelated.Animation.Commands;
using EntitiesRelated.Animation.Events;
using UnityEngine;

namespace GameplayFocused.ExtractingResource.Implementations
{
    /// <summary>
    /// Represents a mineable rock resource that can be gathered by hitting it.
    /// </summary>
    public class MineableRock : HitResourceProvider
    {
        /// <summary>
        /// Delegate for notifying the amount of material gathered per hit.
        /// </summary>
        /// <param name="amount">The amount of material gathered.</param>
        public delegate void OnMaterialGathered(int amount);

        /// <summary>
        /// Event fired when material is gathered.
        /// </summary>
        public event OnMaterialGathered MaterialGathered;

        protected override void Awake()
        {
            base.Awake();
            ActiveObjectRegistry<MineableRock>.Register(this);
            _cooldownTimer.OnTimerStop += () =>
            {
                Register<MineableRock>();
                ResetStages();
            };
        }
        
        public override void Register<T>()
        {
            ActiveObjectRegistry<MineableRock>.Register(this);
        }
        
        public override void Unregister<T>()
        {
            ActiveObjectRegistry<MineableRock>.Unregister(this);
        }
    
        /// <summary>
        /// Gets the interaction animation for mining the rock.
        /// Schedules the ApplyHit callback at 0.5 seconds into the animation.
        /// </summary>
        /// <returns>
        /// A tuple containing the <see cref="AnimationAction"/> and associated <see cref="AnimationEventData"/> array.
        /// </returns>
        public override AnimationCommand GetInteractionAnimation()
        {
            return new MiningAnimationCommand(new List<AnimationEventData>
            {
                new(0.5f, ApplyHit)
            });
        }
        
    
        /// <summary>
        /// Processes a hit on the rock.
        /// Increments the hit count, updates the sprite, and fires the MaterialGathered event.
        /// If the final hit stage is reached, completes the collection.
        /// </summary>
        public override void ApplyHit()
        {
            currentHits++;
            UpdateSpriteBasedOnHits();
        
            // Notify that 1 unit of material has been gathered.
            MaterialGathered?.Invoke(1);
        
            // If the maximum required hits have been reached, complete the collection.
            if (hitStages is { Length: > 0 } && currentHits >= hitStages[^1].hitsRequired)
            {
                CompleteCollection(null); // You may pass the interactor if required.
                Unregister<MineableRock>();
                _cooldownTimer.Start();
            }
        }
    }
}
