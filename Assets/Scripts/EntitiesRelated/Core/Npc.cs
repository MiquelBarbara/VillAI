using EntitiesRelated.Animation;
using EntitiesRelated.Animation.Commands;
using EntitiesRelated.MainCharacter;
using EntitiesRelated.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityServiceLocator;

namespace EntitiesRelated.Core
{
    /// <summary>
    /// Represents a non-player character (NPC) in the game, providing navigation and interaction capabilities.
    /// </summary>
    public class Npc : Character, IMovable
    {
        /// <summary>
        /// The NavMeshAgent used for NPC navigation.
        /// </summary>
        private NavMeshAgent agent;

        private IAnimationCommandReceiver _animationCommandReceiver;
        
        private Vector2 _motionVector;
        
        /// <summary>
        /// Initializes the NPC. If no NavMeshAgent is assigned, it retrieves one from the GameObject.
        /// </summary>
        public override void Start()
        {
            base.Start();
            agent = GetComponent<NavMeshAgent>();
            _animationCommandReceiver = GetComponent<IAnimationCommandReceiver>();
            if(agent == null) return;
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

        /// <summary>
        /// Updates the NPC state each frame by refreshing its motion vector and updating the facing direction.
        /// </summary>
        private void Update()
        {
            if ((agent == null))
            {
                _animationCommandReceiver.ReceiveAnimationCommand(new IdleAnimationCommand());
                return;
            }
            
            if (agent.velocity.sqrMagnitude > 0.01f)
            {
                _animationCommandReceiver.ReceiveAnimationCommand(new WalkAnimationCommand());
                _animationCommandReceiver.UpdateFacing(agent.velocity.x);
                _motionVector = new Vector2(agent.velocity.x, agent.velocity.z).normalized;
            }
            else
            {
                if(!_animationCommandReceiver.IsPlayingAnimation())
                    _animationCommandReceiver.ReceiveAnimationCommand(new IdleAnimationCommand());
            }
        }

        public Vector2 GetVector2D()
        {
            return _motionVector;
        }
        
#if UNITY_EDITOR
        [MenuItem("GameObject/Create NPC")]
        static void CreateNpc() {
            
        }
        
#endif
    }
}
