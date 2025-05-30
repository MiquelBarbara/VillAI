using System;
using EntitiesRelated.Animation.Commands;
using EntitiesRelated.Animation.Events;
using EntitiesRelated.Core;
using GameplayFocused.ActiveObjectsInScene;
using GameplayFocused.ExtractingResource.CollectStrategy;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayFocused.ExtractingResource
{
    public abstract class GatherableResource : RegisterOnEnable<GatherableResource>, Interactable
    {
        [Header("Collection Strategy")]
        protected IResourceCollectionStrategy collectionStrategy;
        public bool Gathered { get; protected set; }
        
        public Item resourceItem;
        
        [Header("Cooldown Configuration")]
        public float interactionCooldown = 5f;
        protected InGameCountdownTimer _cooldownTimer;
        
        protected override void Awake()
        {
            base.Awake();
            _cooldownTimer = new InGameCountdownTimer(interactionCooldown);
        }

        public void Update()
        {
            _cooldownTimer.Tick(Time.deltaTime);
        }
        
        public override void Register<T>()
        {
            ActiveObjectRegistry<T>.Register(this as T);
        }
        
        public override void Unregister<T>()
        {
            ActiveObjectRegistry<T>.Unregister(this as T);
        }

        protected virtual void Gather(Character interactor)
        {
            collectionStrategy?.Collect(interactor, resourceItem, transform.position);
            OnGatherComplete();
        }
    
        protected virtual void OnGatherComplete() { }

        public abstract void Interact(Character interactor);

        public abstract AnimationCommand GetInteractionAnimation();

        public virtual Transform GetTransform() => transform;
    }
}
