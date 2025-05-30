using System.Linq;
using EntitiesRelated.Animation;
using EntitiesRelated.Animation.Commands;
using EntitiesRelated.Animation.Events;
using EntitiesRelated.MainCharacter;
using EntitiesRelated.Core;
using UnityEngine;

namespace EntitiesRelated.Interaction { 

    /// <summary>
    /// Base class for handling interactions with interactable objects in the game.
    /// <summary>
    public class InteractController : MonoBehaviour
    {
        [SerializeField] protected float offsetDistance = 1f;
        [SerializeField] protected float sizeOfInteractableArea = 1.2f;
        protected Rigidbody2D Rigidbody2D;
        private IAnimationCommandReceiver _animationCommandReceiver;
        protected IMovable _movable;
        protected Collider2D[] DetectedColliders { get; set; }

        protected virtual void Awake()
        {
            // Getting the Rigidbody2D component and other necessary components
            Rigidbody2D = GetComponent<Rigidbody2D>();
            _animationCommandReceiver = GetComponent<IAnimationCommandReceiver>();
            _movable = GetComponent<IMovable>();
        }
        
        /// <summary>
        /// Detects the nearest interactable object within a specified area around the character.
        /// </summary>
        /// <returns> The first detected interactable object, or null if none are found.</returns>
        public virtual Interactable DetectInteractable()
        {
            Vector2 position = Rigidbody2D.position + _movable.GetVector2D() * offsetDistance;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);
            DetectedColliders = colliders;
            return colliders.Select(collider => collider.GetComponent<Interactable>())
                .FirstOrDefault(interactable => interactable != null);
        }
        
        /// <summary>
        /// Gets the character that is performing the interaction.
        /// <summary>
        protected virtual Character GetInteractor()
        {
            return GetComponent<Character>();
        }

        /// <summary>
        /// Interacts with the specified interactable object.
        /// </summary>
        /// <param name="interactable"> The interactable object to interact with.</param>
        /// <returns> True if the interaction was successful, false otherwise.</returns>
        public virtual bool Interact(Interactable interactable)
        {
            AnimationCommand animationCommand= interactable.GetInteractionAnimation();
            _animationCommandReceiver.ReceiveAnimationCommand(animationCommand);
            interactable.Interact(GetInteractor());
            return true;
        }
        
        /// <summary>
        ///  Attempts to interact with the nearest interactable object detected within the specified area.
        /// <summary>
        public virtual bool Interact()
        {
            var interactable = DetectInteractable();
            if (interactable != null)
            {
                // If an interactable object is found, trigger the interaction
                AnimationCommand animationCommand= interactable.GetInteractionAnimation();
                _animationCommandReceiver.ReceiveAnimationCommand(animationCommand); // Send animation command to the receiver
                interactable.Interact(GetInteractor());
                return true;
            }

            Debug.Log("No interactable object found.");
            return false;
        }
    }
}