using EntitiesRelated.Animation.Events; using EntitiesRelated.Interaction; using EntitiesRelated.Core; using GameplayFocused.TilemapManagment; using UnityEngine;
using UnityEngine.InputSystem;

namespace EntitiesRelated.MainCharacter { 
    
    /// <summary>
    /// Controls character interaction with objects in a 2D environment.
    /// </summary>
    public class CharacterInteractController : InteractController
    {
        private HighlightController _highlightController;
        [SerializeField] private GameObject highlighter;

        private Vector2 lastInput;
        protected override void Awake()
        {
            base.Awake();
            _highlightController = new HighlightController(highlighter);
            
        }
        
        private void Update()
        {
            CheckForInteractables();
        }
        
        /// <summary>
        /// Initiates interaction with the detected interactable object.
        /// </summary>
        /// <param name="context"> The input context containing interaction data.</param>
        public void Interact(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Interact();
            }
        }

        /// <summary>
        /// Interacts with the detected interactable object if one is present.
        /// </summary>
        private void CheckForInteractables()
        {
            Interactable interactable = DetectInteractable();
            if (interactable != null)
            {
                _highlightController.Hightlight(interactable.GetTransform());
            }
            else
            {
                _highlightController.Hide();
            }
        }
        
        /// <summary>
        /// Detects the nearest interactable object within a specified area around the character.
        /// </summary>
        /// <returns> The first detected interactable object, or null if none are found.</returns>
        public override Interactable DetectInteractable()
        {
            Interactable interactable = base.DetectInteractable();
            if (interactable != null)
                return interactable;

            Vector2 interactionPoint = Rigidbody2D.position + _movable.GetVector2D() * offsetDistance;
            Interactable tileWrapper = FarmingTilemapManager.Instance.GetTileWrapperAtPoint(interactionPoint);
            return tileWrapper;
        }
    }
}