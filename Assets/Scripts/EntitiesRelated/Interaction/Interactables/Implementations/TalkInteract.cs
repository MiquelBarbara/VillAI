using EntitiesRelated.Animation.Commands;
using EntitiesRelated.Animation.Events;
using EntitiesRelated.Core;
using Game.ALPHA;
using GameplayFocused;
using GameplayFocused.ActiveObjectsInScene;
using GOAP.Scripts;
using GOAP.Scripts.AgentStats;
using NPCs;
using Systems.DialogueSystem;
using UnityEngine;

namespace EntitiesRelated.Interaction.Interactables.Implementations
{
    /// <summary>
    /// Handles talk interactions between the player character and NPCs by initiating dialogue sessions.
    /// </summary>
    public class TalkInteract : RegisterOnEnable<TalkInteract>, Interactable
    {
        private Npc _npc;
        private IAgentManager _agentManager;
        
        /// <summary>
        /// Initializes the TalkInteract component by retrieving necessary components and services.
        /// </summary>
        private void Start()
        {
            _npc = GetComponent<Npc>();
            _agentManager = new AgentManager();
        }

        /// <summary>
        /// Gets the position of the interactable object.
        /// </summary>
        public Vector3 Position => gameObject.transform.position;

        /// <summary>
        /// Gets the target game object for the interaction.
        /// </summary>
        public GameObject target => gameObject;

        /// <summary>
        /// Initiates a dialogue session between the interacting character and the NPC.
        /// </summary>
        /// <param name="character">The character initiating the interaction.</param>
        public void Interact(Character character)
        {
            // TODO: State Machine for the user so it can't move.
            _agentManager.StopAgents(character, _npc);
            ActiveObjectRegistry<TalkInteract>.Unregister(this);

            var session = StartDialogueSession(character, _npc);
            session.OnSessionEnd += () => OnDialogueSessionEnd(character, session);
        }

        /// <summary>
        /// Gets the animation action and associated events for this interaction.
        /// </summary>
        /// <returns>
        /// A tuple containing the <see cref="AnimationAction"/> (idle) and a null array for events.
        /// </returns>
        public AnimationCommand GetInteractionAnimation()
        {
            return new IdleAnimationCommand();
        }

        /// <summary>
        /// Gets the transform of the interactable object.
        /// </summary>
        /// <returns>The transform component of the game object.</returns>
        public Transform GetTransform()
        {
            return gameObject.transform;
        }

        /// <summary>
        /// Callback executed when the dialogue session ends.
        /// Resumes agent activities and updates the character's socialization stats.
        /// </summary>
        /// <param name="character">The character that was interacting.</param>
        /// <param name="session">The dialogue session that ended.</param>
        private void OnDialogueSessionEnd(Character character, Session session)
        {
            ActiveObjectRegistry<TalkInteract>.Register(this);
            
            _agentManager.ResumeAgents(character, _npc);
            
        }
        
        public Session StartDialogueSession(Character character1, Character character2)
        {
            if (character1.TryGetComponent<InventoryController>(out var component))
            {
                component.CloseToolbar();
            }
            
            _agentManager.StopAgents(character1, character2.GetComponent<Npc>());
            
            var session = new Session(character1, character2);
            GameManager.Instance.ConversationSystem.StartSession(session);

            return session;
        }

        public override void Register<T>()
        {
            ActiveObjectRegistry<TalkInteract>.Register(this);
        }

        public override void Unregister<T>()
        {
            ActiveObjectRegistry<TalkInteract>.Unregister(this);
        }
    }
}
