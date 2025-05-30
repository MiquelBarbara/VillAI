using System;
using EntitiesRelated.Interaction.Interactables.Implementations;
using UnityEngine;

namespace GOAP.Scripts
{
    public class StoreStrategy : IActionStrategy
    {
        private GameObject agent;
        private Func<bool> interrumpt;
        private TalkInteract talkInteract;
        private StoreInteract storeInteract;

        public StoreStrategy(GameObject agent, Func<bool> interrumpt)
        {
            this.agent = agent;
            this.interrumpt = interrumpt;
            Complete = false;
            
            // Try to get existing components (if any)
            talkInteract = agent.GetOrAdd<TalkInteract>();
            storeInteract = agent.GetOrAdd<StoreInteract>();
        }

        public bool CanPerform => true;
        public bool Complete { get; private set; }

        public void Start()
        {
            // Remove TalkInteract if it exists
            if (talkInteract != null)
            {
                UnityEngine.Object.Destroy(talkInteract);
                talkInteract = null;
            }
            
            // Add StoreInteract (if not already present)
            if (storeInteract == null)
            {
                storeInteract = agent.AddComponent<StoreInteract>();
            }
        }

        public void Update(float deltaTime)
        {
            // Check the interrupt condition
            if (!interrumpt())
                return;

            // If interrupt condition is met, remove StoreInteract and add TalkInteract
            if (storeInteract != null)
            {
                UnityEngine.Object.Destroy(storeInteract);
                storeInteract = null;
            }
            
            if (talkInteract == null)
            {
                talkInteract = agent.AddComponent<TalkInteract>();
            }
            
            Complete = true;
        }

        public void Stop()
        {
            // Ensure the agent returns to its default state
            if (storeInteract != null)
            {
                UnityEngine.Object.Destroy(storeInteract);
                storeInteract = null;
            }
            
            if (talkInteract == null)
            {
                talkInteract = agent.AddComponent<TalkInteract>();
            }
        }
    }
}
