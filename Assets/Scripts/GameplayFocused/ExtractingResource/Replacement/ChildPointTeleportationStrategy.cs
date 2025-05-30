using EntitiesRelated.MainCharacter;
using EntitiesRelated.Core;
using UnityEngine;

namespace GameplayFocused.ExtractingResource.Replacement
{
    /// <summary>
    /// Implements a teleportation strategy that moves a character to one of two designated child points
    /// (left or right) based on the character's horizontal position relative to a resource.
    /// </summary>
    public class ChildPointTeleportationStrategy : ITeleportationStrategy
    {
        /// <summary>
        /// The left teleportation point.
        /// </summary>
        private readonly Transform _leftPoint;
    
        /// <summary>
        /// The right teleportation point.
        /// </summary>
        private readonly Transform _rightPoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChildPointTeleportationStrategy"/> class.
        /// </summary>
        /// <param name="left">The transform representing the left teleportation point.</param>
        /// <param name="right">The transform representing the right teleportation point.</param>
        public ChildPointTeleportationStrategy(Transform left, Transform right)
        {
            _leftPoint = left;
            _rightPoint = right;
        }

        /// <summary>
        /// Teleports the specified character to a child point based on its position relative to the resource.
        /// If the character's x-position is greater than or equal to the resource's x-position, it is moved to the right point;
        /// otherwise, to the left point. A minimal movement vector is applied to ensure movement is registered.
        /// </summary>
        /// <param name="interactor">The character to be teleported.</param>
        /// <param name="resourcePosition">The position of the resource triggering the teleport.</param>
        public void Teleport(Character interactor, Vector3 resourcePosition)
        {
            var movable = interactor.GetComponent<IMovable>();
            // Reposition based on interactor's x-position relative to the resource.
            if (interactor.transform.position.x >= resourcePosition.x)
            {
                interactor.transform.position = _rightPoint.position;
                //movable?.Move(new Vector2(-0.001f, 0));
            }
            else
            {
                interactor.transform.position = _leftPoint.position;
                //movable?.Move(new Vector2(0.001f, 0));
            }
        }
    }
}
