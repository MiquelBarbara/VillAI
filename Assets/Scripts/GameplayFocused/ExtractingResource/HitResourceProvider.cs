using EntitiesRelated.Animation.Events;
using EntitiesRelated.Core;
using GameplayFocused.ExtractingResource.CollectStrategy;
using GameplayFocused.ExtractingResource.Replacement;
using UnityEngine;

namespace GameplayFocused.ExtractingResource
{
    /// <summary>
    /// Abstract base class for resources that require multiple hits to gather.
    /// Implements common logic for updating the sprite based on hit count and triggering collection.
    /// </summary>
    public abstract class HitResourceProvider : GatherableResource
    {
        [SerializeField] private GameObject collectableItemPrefab;
    
        [Header("Hit Data")] public HitStageData[] hitStages;

        /// <summary>
        /// The SpriteRenderer used to display the resource.
        /// </summary>
        protected SpriteRenderer spriteRenderer;
    
        [Header("Interaction Points")] public Transform leftPoint;

        /// <summary>
        /// The right point for teleportation or interaction.
        /// </summary>
        public Transform rightPoint;

        /// <summary>
        /// The teleportation strategy used to move the interactor to the resource.
        /// </summary>
        protected ITeleportationStrategy teleportStrategy;
    
        /// <summary>
        /// The current number of hits accumulated.
        /// </summary>
        protected int currentHits = 0;
    
        /// <summary>
        /// Initializes the component by setting up the sprite renderer, teleportation strategy, and collection strategy.
        /// Also sets the initial sprite based on the first hit stage.
        /// </summary>
        protected virtual void Awake()
        {
            base.Awake();
            spriteRenderer = GetComponent<SpriteRenderer>();
            teleportStrategy = new ChildPointTeleportationStrategy(leftPoint, rightPoint);
        
            // Assign a default collection strategy if none is set.
            collectionStrategy ??= new WorldDropCollectionStrategy(collectableItemPrefab);
        
            // Initialize the sprite to the first hit stage if available.
            if (hitStages != null && hitStages.Length > 0 && hitStages[0].sprite != null)
            {
                spriteRenderer.sprite = hitStages[0].sprite;
            }
        }
        
        protected void ResetStages()
        {
            currentHits = 0;
            UpdateSpriteBasedOnHits();
        }
    
        /// <summary>
        /// Executes the common interaction logic by teleporting the interactor to the resource.
        /// The actual "hit" action is expected to be triggered via animation events calling <see cref="ApplyHit"/>.
        /// </summary>
        /// <param name="interactor">The character interacting with the resource.</param>
        public override void Interact(Character interactor)
        {
            teleportStrategy.Teleport(interactor, transform.position);
            // The hit action will be triggered through animation events which call ApplyHit.
        }
    
        
        /// <summary>
        /// Processes a hit on the resource.
        /// Derived classes must implement this method to handle hit logic.
        /// </summary>
        public abstract void ApplyHit();
    
        /// <summary>
        /// Completes the resource gathering process once the required hit count is reached.
        /// </summary>
        /// <param name="interactor">The character that has gathered the resource.</param>
        protected void CompleteCollection(Character interactor)
        {
            Gather(interactor);
        }
        
        /// <summary>
        /// Updates the displayed sprite based on the current number of hits.
        /// Iterates through the hit stages and assigns the appropriate sprite.
        /// </summary>
        protected void UpdateSpriteBasedOnHits()
        {
            if (hitStages == null || hitStages.Length == 0) return;
        
            // Iterate through each stage (assumes hitStages is ordered by increasing hitsRequired).
            foreach (var stage in hitStages)
            {
                if (currentHits >= stage.hitsRequired) continue;
                spriteRenderer.sprite = stage.sprite;
                return;
            }
        
            // If all stages are exceeded, assign the sprite from the last stage.
            spriteRenderer.sprite = hitStages[hitStages.Length - 1].sprite;
        }
    }
}
