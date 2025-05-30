using System.Collections.Generic;
using EntitiesRelated.Animation.Commands;
using EntitiesRelated.Animation.Events;
using UnityEngine;

namespace GameplayFocused.ExtractingResource.Implementations
{
    /// <summary>
    /// Represents a tree that can be chopped to gather wood.
    /// </summary>
    public class TreeChoppable : HitResourceProvider
    {

        [SerializeField] private GameObject log;
        /// <summary>
        /// Delegate for notifying the amount of wood gathered per chop.
        /// </summary>
        /// <param name="amount">The amount of wood gathered.</param>
        public delegate void OnWoodGathered(int amount);

        protected override void Awake()
        {
            base.Awake();
            ActiveObjectRegistry<TreeChoppable>.Register(this);
            _cooldownTimer.OnTimerStop += () =>
            {
                Register<TreeChoppable>();
                ResetStages();
                log.SetActive(true);
            };
        }
        
        public override void Register<T>()
        {
            ActiveObjectRegistry<TreeChoppable>.Register(this);
        }
        
        public override void Unregister<T>()
        {
            ActiveObjectRegistry<TreeChoppable>.Unregister(this);
        }

        /// <summary>
        /// Event fired when wood is gathered.
        /// </summary>
        public event OnWoodGathered WoodGathered;
    
        [Header("Wood Configuration")] public int woodPerChop = 1;
    
        /// <summary>
        /// Gets the interaction animation for chopping the tree.
        /// Defines the axe animation and schedules the ApplyHit callback.
        /// </summary>
        /// <returns>
        /// A tuple containing the <see cref="AnimationAction"/> and associated <see cref="AnimationEventData"/> array.
        /// </returns>
        public override AnimationCommand GetInteractionAnimation()
        {
            var animationEvents = new[]
            {
                new AnimationEventData(0.5f, ApplyHit)
            };
            return new AxeAnimationCommand(new List<AnimationEventData>
            {
                new(0.5f, ApplyHit)
            });
        }

        /// <summary>
        /// Processes a hit on the tree.
        /// Increments the hit count, updates the sprite, and fires the WoodGathered event.
        /// If the final hit stage is reached, completes the collection.
        /// </summary>
        public override void ApplyHit()
        {
            // Increment the number of accumulated hits.
            currentHits++;
            // Update the sprite based on defined hit stages.
            UpdateSpriteBasedOnHits();
        
            // Notify that wood has been gathered for this hit.
            WoodGathered?.Invoke(woodPerChop);
        
            // If the maximum required hits have been reached, complete the collection.
            if (hitStages != null && hitStages.Length > 0 && currentHits >= hitStages[^1].hitsRequired)
            {
                log.SetActive(false);
                Unregister<TreeChoppable>();
                _cooldownTimer.Start();
                CompleteCollection(null); // You may pass the interactor if required.
            }
        }
    }
}
