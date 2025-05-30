using EntitiesRelated.Animation.Events; using EntitiesRelated.Interaction; using EntitiesRelated.Core; using GameplayFocused.TilemapManagment; using UnityEngine;
using UnityEngine.InputSystem;

namespace EntitiesRelated.MainCharacter { 
    // Hereda de InteractController para utilizar la lógica común, añadiendo el highlighter para feedback.
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
        
        public void Interact(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Interact();
            }
        }

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