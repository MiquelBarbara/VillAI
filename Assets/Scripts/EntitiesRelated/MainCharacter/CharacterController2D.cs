using EntitiesRelated.Animation;
using EntitiesRelated.Animation.Commands;
using EntitiesRelated.Interaction;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EntitiesRelated.MainCharacter
{
    /// <summary>
    /// Controls character movement and interaction in a 2D environment.
    /// </summary>
    public class CharacterController2D : MonoBehaviour, IMovable
    {
        /// <summary>
        /// Reference to the Rigidbody2D component for physics calculations.
        /// </summary>
        [SerializeField] private Rigidbody2D _rigidbody2D;
        private IAnimationCommandReceiver _animationCommandReceiver;
        /// <summary>
        /// The movement speed of the character.
        /// </summary>
        [Range(0, 100)]
        [SerializeField] private float speed = 5f;
        
        private Vector2 _moveInput;

        /// <summary>
        /// Initializes the character movement and retrieves required components.
        /// </summary>
        private void Awake()
        {
            _animationCommandReceiver = GetComponent<IAnimationCommandReceiver>();
        }
    
        /// <summary>
        /// Handles character movement input.
        /// </summary>
        /// <param name="context">The input context containing movement vector data.</param>
        public void Move(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
            _rigidbody2D.velocity = _moveInput.normalized * speed;

            _animationCommandReceiver.ReceiveAnimationCommand(new WalkAnimationCommand()); //Need to communicate to AnimatorController
        
            if (_moveInput != Vector2.zero && _moveInput.x != 0f)
            {
                _animationCommandReceiver.UpdateFacing(_moveInput.x); //Need to communicate to AnimatorController
            }
        
            if (_moveInput == Vector2.zero)
            {
                _animationCommandReceiver.ReceiveAnimationCommand(new IdleAnimationCommand()); //Need to communicate to AnimatorController
            }
        }

        public Vector2 GetVector2D()
        {
            return _moveInput;
        }
    }
}
