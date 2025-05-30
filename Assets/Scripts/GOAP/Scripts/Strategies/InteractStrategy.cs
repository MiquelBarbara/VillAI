using System;
using EntitiesRelated.Interaction;
using EntitiesRelated.Core;
using UnityEngine;

namespace GOAP.Scripts
{
    public class InteractStrategy: IActionStrategy
    {
        private Character character;
        private Func<Interactable> interactable;
        private bool performed;
        private CountdownTimer cooldownTimer;
        
        public InteractStrategy(Character character, Func<Interactable> interactable, float cooldown = 4f)
        {
            this.character = character;
            this.interactable = interactable;
            cooldownTimer = new CountdownTimer(cooldown);
        }
        
        public void Start()
        {
            character.GetComponent<InteractController>().Interact(interactable());
            performed = false;
            cooldownTimer.OnTimerStop += () => performed = true;
            cooldownTimer.Start();
        }
        
        public void Update(float deltaTime)
        {
            cooldownTimer.Tick(deltaTime);
        }
        
        public bool CanPerform => !Complete;
        public bool Complete => performed;
    }
}