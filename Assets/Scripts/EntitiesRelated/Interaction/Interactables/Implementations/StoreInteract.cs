using EntitiesRelated.Animation.Commands;
using EntitiesRelated.Animation.Events;
using EntitiesRelated.Core;
using GOAP.Scripts;
using NPCs;
using Systems.DialogueSystem;
using UnityEngine;

namespace EntitiesRelated.Interaction.Interactables.Implementations
{
    /// <summary>
    /// Handles store interactions between the player character and NPCs, initiating trade sessions.
    /// </summary>
    [RequireComponent(typeof(Character))]
    public class StoreInteract : MonoBehaviour, Interactable
    {
        private Npc _npc;
        private IAgentManager _agentManager;
        private ITradeSessionService _tradeSessionService;
        private const string StatSocializeKey = "Socialization";

        /// <summary>
        /// Initializes the StoreInteract component by retrieving necessary components and services.
        /// </summary>
        private void Start()
        {
            _npc = GetComponent<Npc>();
            _agentManager = new AgentManager();
            _tradeSessionService = new TradeSessionService();
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
        /// Initiates a trade session between the interacting character and the NPC.
        /// </summary>
        /// <param name="character">The character initiating the interaction.</param>
        public void Interact(Character character)
        {
            // TODO: State Machine for the user so it can't move.
            _agentManager.StopAgents(character, _npc);

            var session = _tradeSessionService.StartTradeSession(character, _npc);
            session.OnSessionEnd += () => OnTradeSessionEnd(character, session);
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
        /// Callback executed when the trade session ends, resuming agent activities.
        /// </summary>
        /// <param name="character">The character that was interacting.</param>
        /// <param name="session">The trade session that ended.</param>
        private void OnTradeSessionEnd(Character character, Session session)
        {
            _agentManager.ResumeAgents(character, _npc);
        }
    }
}
